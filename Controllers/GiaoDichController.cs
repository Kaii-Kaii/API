using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QL_ThuChi.Data;
using QL_ThuChi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QL_ThuChi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiaoDichController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GiaoDichController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/GiaoDich
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GiaoDich>>> GetGiaoDichs()
        {
            return await _context.GiaoDichs
                .Include(g => g.KhachHang)
                .Include(g => g.Vi)
                .Include(g => g.ViNhan)
                .Include(g => g.HangMuc)
                .ToListAsync();
        }

        // GET: api/GiaoDich/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GiaoDich>> GetGiaoDich(int id)
        {
            var giaoDich = await _context.GiaoDichs
                .Include(g => g.KhachHang)
                .Include(g => g.Vi)
                .Include(g => g.ViNhan)
                .Include(g => g.HangMuc)
                .FirstOrDefaultAsync(g => g.MaGiaoDich == id);

            if (giaoDich == null)
            {
                return NotFound();
            }

            return giaoDich;
        }

        // GET: api/GiaoDich/user/{maNguoiDung}
        [HttpGet("user/{maNguoiDung}")]
        public async Task<ActionResult<IEnumerable<GiaoDich>>> GetGiaoDichsByUser(string maNguoiDung)
        {
            return await _context.GiaoDichs
                .Include(g => g.KhachHang)
                .Include(g => g.Vi)
                .Include(g => g.ViNhan)
                .Include(g => g.HangMuc)
                .Where(g => g.MaNguoiDung == maNguoiDung)
                .ToListAsync();
        }

        // POST: api/GiaoDich
        [HttpPost]
        public async Task<ActionResult<GiaoDich>> CreateGiaoDich(GiaoDichCreateDto giaoDichDto)
        {
            // Validate user exists
            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(k => k.MAKH == giaoDichDto.MaNguoiDung);
            if (khachHang == null)
            {
                return BadRequest(new { error = "Không tìm thấy người dùng" });
            }

            // Validate wallet exists
            var viNguoiDung = await _context.ViNguoiDungs
                .FirstOrDefaultAsync(v => v.MaNguoiDung == giaoDichDto.MaNguoiDung && v.MaVi == giaoDichDto.MaVi);
            if (viNguoiDung == null)
            {
                return BadRequest(new { error = "Không tìm thấy ví của người dùng" });
            }

            // Validate category if provided
            if (!string.IsNullOrEmpty(giaoDichDto.MAHANGMUC))
            {
                var danhMuc = await _context.HangMucs
                    .FirstOrDefaultAsync(d => d.MAHANGMUC == giaoDichDto.MAHANGMUC &&
                                           d.MaNguoiDung == giaoDichDto.MaNguoiDung);
                if (danhMuc == null)
                {
                    return BadRequest(new { error = "Không tìm thấy danh mục" });
                }
            }

            // Validate destination wallet for transfers
            if (giaoDichDto.LoaiGiaoDich == "ChuyenKhoan")
            {
                if (!giaoDichDto.MaViNhan.HasValue || giaoDichDto.MaViNhan == 0)
                {
                    return BadRequest(new { error = "Vui lòng chọn ví nhận tiền" });
                }

                var viNhan = await _context.ViNguoiDungs
                    .FirstOrDefaultAsync(v => v.MaNguoiDung == giaoDichDto.MaNguoiDung &&
                                           v.MaVi == giaoDichDto.MaViNhan);
                if (viNhan == null)
                {
                    return BadRequest(new { error = "Không tìm thấy ví nhận tiền" });
                }

                if (giaoDichDto.MaVi == giaoDichDto.MaViNhan)
                {
                    return BadRequest(new { error = "Không thể chuyển tiền vào cùng một ví" });
                }
            }
            else
            {
                // Set maViNhan to null for non-transfer transactions
                giaoDichDto.MaViNhan = null;
            }

            var giaoDich = new GiaoDich
            {
                MaNguoiDung = giaoDichDto.MaNguoiDung,
                MaVi = giaoDichDto.MaVi,
                MaHangMuc = giaoDichDto.MAHANGMUC,
                SoTien = giaoDichDto.SoTien,
                GhiChu = giaoDichDto.GhiChu,
                NgayGiaoDich = giaoDichDto.NgayGiaoDich,
                LoaiGiaoDich = giaoDichDto.LoaiGiaoDich,
                MaViNhan = giaoDichDto.MaViNhan
            };

            giaoDich.SoTienCu = viNguoiDung.SoDu;

            if (giaoDichDto.LoaiGiaoDich == "Thu")
            {
                viNguoiDung.SoDu += giaoDichDto.SoTien;
            }
            else if (giaoDichDto.LoaiGiaoDich == "Chi")
            {
                if (viNguoiDung.SoDu < giaoDichDto.SoTien)
                {
                    return BadRequest(new { error = "Số dư không đủ" });
                }
                viNguoiDung.SoDu -= giaoDichDto.SoTien;
            }
            else if (giaoDichDto.LoaiGiaoDich == "ChuyenKhoan")
            {
                if (viNguoiDung.SoDu < giaoDichDto.SoTien)
                {
                    return BadRequest(new { error = "Số dư không đủ" });
                }

                var viNhan = await _context.ViNguoiDungs
                    .FirstOrDefaultAsync(v => v.MaNguoiDung == giaoDichDto.MaNguoiDung &&
                                           v.MaVi == giaoDichDto.MaViNhan);

                viNguoiDung.SoDu -= giaoDichDto.SoTien;
                viNhan.SoDu += giaoDichDto.SoTien;
            }

            giaoDich.SoTienMoi = viNguoiDung.SoDu;

            _context.GiaoDichs.Add(giaoDich);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGiaoDich), new { id = giaoDich.MaGiaoDich }, giaoDich);
        }

        // PUT: api/GiaoDich/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGiaoDich(int id, GiaoDichUpdateDto giaoDichDto)
        {
            var giaoDich = await _context.GiaoDichs.FindAsync(id);
            if (giaoDich == null)
            {
                return NotFound();
            }

            // Update wallet balance
            var viNguoiDung = await _context.ViNguoiDungs
                .FirstOrDefaultAsync(v => v.MaNguoiDung == giaoDich.MaNguoiDung && v.MaVi == giaoDich.MaVi);

            if (viNguoiDung == null)
            {
                return BadRequest(new { error = "Không tìm thấy ví của người dùng" });
            }

            // Revert the old transaction
            if (giaoDich.LoaiGiaoDich == "Thu")
            {
                viNguoiDung.SoDu -= giaoDich.SoTien;
            }
            else if (giaoDich.LoaiGiaoDich == "Chi")
            {
                viNguoiDung.SoDu += giaoDich.SoTien;
            }
            else if (giaoDich.LoaiGiaoDich == "ChuyenKhoan")
            {
                var viNhan = await _context.ViNguoiDungs
                    .FirstOrDefaultAsync(v => v.MaNguoiDung == giaoDich.MaNguoiDung && v.MaVi == giaoDich.MaViNhan);

                if (viNhan != null)
                {
                    viNguoiDung.SoDu += giaoDich.SoTien;
                    viNhan.SoDu -= giaoDich.SoTien;
                }
            }

            // Apply the new transaction
            if (giaoDichDto.MaVi.HasValue) giaoDich.MaVi = giaoDichDto.MaVi.Value;
            if (!string.IsNullOrEmpty(giaoDichDto.MAHANGMUC)) giaoDich.MaHangMuc = giaoDichDto.MAHANGMUC;
            if (giaoDichDto.SoTien.HasValue) giaoDich.SoTien = giaoDichDto.SoTien.Value;
            if (giaoDichDto.GhiChu != null) giaoDich.GhiChu = giaoDichDto.GhiChu;
            if (giaoDichDto.NgayGiaoDich.HasValue) giaoDich.NgayGiaoDich = giaoDichDto.NgayGiaoDich.Value;
            if (giaoDichDto.LoaiGiaoDich != null) giaoDich.LoaiGiaoDich = giaoDichDto.LoaiGiaoDich;
            if (giaoDichDto.MaViNhan.HasValue) giaoDich.MaViNhan = giaoDichDto.MaViNhan;

            giaoDich.SoTienCu = viNguoiDung.SoDu;

            if (giaoDich.LoaiGiaoDich == "Thu")
            {
                viNguoiDung.SoDu += giaoDich.SoTien;
            }
            else if (giaoDich.LoaiGiaoDich == "Chi")
            {
                if (viNguoiDung.SoDu < giaoDich.SoTien)
                {
                    return BadRequest(new { error = "Số dư không đủ" });
                }
                viNguoiDung.SoDu -= giaoDich.SoTien;
            }
            else if (giaoDich.LoaiGiaoDich == "ChuyenKhoan")
            {
                if (viNguoiDung.SoDu < giaoDich.SoTien)
                {
                    return BadRequest(new { error = "Số dư không đủ" });
                }

                var viNhan = await _context.ViNguoiDungs
                    .FirstOrDefaultAsync(v => v.MaNguoiDung == giaoDich.MaNguoiDung && v.MaVi == giaoDich.MaViNhan);

                if (viNhan == null)
                {
                    return BadRequest(new { error = "Không tìm thấy ví nhận tiền" });
                }

                viNguoiDung.SoDu -= giaoDich.SoTien;
                viNhan.SoDu += giaoDich.SoTien;
            }

            giaoDich.SoTienMoi = viNguoiDung.SoDu;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GiaoDichExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Get updated transaction details
            var updatedGiaoDich = await _context.GiaoDichs
                .Include(g => g.KhachHang)
                .Include(g => g.Vi)
                .Include(g => g.ViNhan)
                .Include(g => g.HangMuc)
                .FirstOrDefaultAsync(g => g.MaGiaoDich == id);

            return Ok(new
            {
                message = "Cập nhật giao dịch thành công",
                giaoDich = updatedGiaoDich
            });
        }

        // DELETE: api/GiaoDich/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGiaoDich(int id)
        {
            var giaoDich = await _context.GiaoDichs.FindAsync(id);
            if (giaoDich == null)
            {
                return NotFound();
            }

            // Delete related LichSuGiaoDich records first
            var lichSuGiaoDichs = await _context.LichSuGiaoDichs
                .Where(l => l.MaGiaoDich == id)
                .ToListAsync();
            _context.LichSuGiaoDichs.RemoveRange(lichSuGiaoDichs);

            // Delete related AnhHoaDon records
            var anhHoaDons = await _context.AnhHoaDons
                .Where(a => a.MaGiaoDich == id)
                .ToListAsync();
            _context.AnhHoaDons.RemoveRange(anhHoaDons);

            // Revert the transaction
            var viNguoiDung = await _context.ViNguoiDungs
                .FirstOrDefaultAsync(v => v.MaNguoiDung == giaoDich.MaNguoiDung && v.MaVi == giaoDich.MaVi);

            if (viNguoiDung == null)
            {
                return BadRequest(new { error = "Không tìm thấy ví của người dùng" });
            }

            if (giaoDich.LoaiGiaoDich == "Thu")
            {
                viNguoiDung.SoDu -= giaoDich.SoTien;
            }
            else if (giaoDich.LoaiGiaoDich == "Chi")
            {
                viNguoiDung.SoDu += giaoDich.SoTien;
            }
            else if (giaoDich.LoaiGiaoDich == "ChuyenKhoan")
            {
                var viNhan = await _context.ViNguoiDungs
                    .FirstOrDefaultAsync(v => v.MaNguoiDung == giaoDich.MaNguoiDung && v.MaVi == giaoDich.MaViNhan);

                if (viNhan != null)
                {
                    viNguoiDung.SoDu += giaoDich.SoTien;
                    viNhan.SoDu -= giaoDich.SoTien;
                }
            }

            _context.GiaoDichs.Remove(giaoDich);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Xóa giao dịch và dữ liệu liên kết thành công",
                soAnhHoaDonDaXoa = anhHoaDons.Count,
                soLichSuDaXoa = lichSuGiaoDichs.Count
            });
        }

        private bool GiaoDichExists(int id)
        {
            return _context.GiaoDichs.Any(e => e.MaGiaoDich == id);
        }
    }
}