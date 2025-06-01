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
    public class LichSuGiaoDichController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LichSuGiaoDichController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/LichSuGiaoDich
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LichSuGiaoDich>>> GetLichSuGiaoDichs()
        {
            return await _context.LichSuGiaoDichs
                .Include(l => l.GiaoDich)
                .Include(l => l.KhachHang)
                .ToListAsync();
        }

        // GET: api/LichSuGiaoDich/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LichSuGiaoDich>> GetLichSuGiaoDich(int id)
        {
            var lichSuGiaoDich = await _context.LichSuGiaoDichs
                .Include(l => l.GiaoDich)
                .Include(l => l.KhachHang)
                .FirstOrDefaultAsync(l => l.MaLichSu == id);

            if (lichSuGiaoDich == null)
            {
                return NotFound();
            }

            return lichSuGiaoDich;
        }

        // GET: api/LichSuGiaoDich/giaodich/{maGiaoDich}
        [HttpGet("giaodich/{maGiaoDich}")]
        public async Task<ActionResult<IEnumerable<LichSuGiaoDich>>> GetLichSuGiaoDichsByGiaoDich(int maGiaoDich)
        {
            return await _context.LichSuGiaoDichs
                .Include(l => l.GiaoDich)
                .Where(l => l.MaGiaoDich == maGiaoDich)
                .OrderByDescending(l => l.ThoiGian)
                .ToListAsync();
        }

        // GET: api/LichSuGiaoDich/nguoidung/{maNguoiDung}
        [HttpGet("nguoidung/{maNguoiDung}")]
        public async Task<ActionResult<IEnumerable<LichSuGiaoDich>>> GetLichSuGiaoDichsByUser(string maNguoiDung)
        {
            return await _context.LichSuGiaoDichs
                .Include(l => l.GiaoDich)
                .Include(l => l.KhachHang)
                .Where(l => l.GiaoDich.MaNguoiDung == maNguoiDung)
                .ToListAsync();
        }

        // GET: api/LichSuGiaoDich/nguoidung/{maNguoiDung}/thoigian
        [HttpGet("nguoidung/{maNguoiDung}/thoigian")]
        public async Task<ActionResult<IEnumerable<LichSuGiaoDich>>> GetLichSuGiaoDichsByUserAndTime(
            string maNguoiDung, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            return await _context.LichSuGiaoDichs
                .Include(l => l.GiaoDich)
                .Include(l => l.KhachHang)
                .Where(l => l.GiaoDich.MaNguoiDung == maNguoiDung &&
                           l.ThoiGian >= startDate &&
                           l.ThoiGian <= endDate)
                .ToListAsync();
        }

        // POST: api/LichSuGiaoDich
        [HttpPost]
        public async Task<ActionResult<LichSuGiaoDich>> CreateLichSuGiaoDich(LichSuGiaoDichCreateDto lichSuGiaoDichDto)
        {
            // Kiểm tra giao dịch
            var giaoDich = await _context.GiaoDichs.FindAsync(lichSuGiaoDichDto.MaGiaoDich);
            if (giaoDich == null)
            {
                return BadRequest("Không tìm thấy giao dịch");
            }

            // Kiểm tra người dùng
            var khachHang = await _context.KhachHangs.FindAsync(lichSuGiaoDichDto.ThucHienBoi);
            if (khachHang == null)
            {
                return BadRequest("Không tìm thấy người dùng");
            }

            // Kiểm tra hành động
            if (!IsValidHanhDong(lichSuGiaoDichDto.HanhDong))
            {
                return BadRequest("Hành động không hợp lệ. Chỉ chấp nhận: TaoMoi, CapNhat, Xoa");
            }

            // Set thời gian mặc định nếu không có
            if (lichSuGiaoDichDto.ThoiGian == default)
            {
                lichSuGiaoDichDto.ThoiGian = DateTime.Now;
            }

            // Tạo mới lịch sử giao dịch
            var newLichSuGiaoDich = new LichSuGiaoDich
            {
                MaGiaoDich = lichSuGiaoDichDto.MaGiaoDich,
                HanhDong = lichSuGiaoDichDto.HanhDong,
                SoTienCu = lichSuGiaoDichDto.SoTienCu,
                SoTienMoi = lichSuGiaoDichDto.SoTienMoi,
                ThucHienBoi = lichSuGiaoDichDto.ThucHienBoi,
                ThoiGian = lichSuGiaoDichDto.ThoiGian,
                GiaoDich = giaoDich,
                KhachHang = khachHang
            };

            _context.LichSuGiaoDichs.Add(newLichSuGiaoDich);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLichSuGiaoDich), new { id = newLichSuGiaoDich.MaLichSu }, newLichSuGiaoDich);
        }

        // PUT: api/LichSuGiaoDich/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLichSuGiaoDich(int id, LichSuGiaoDich lichSuGiaoDich)
        {
            if (id != lichSuGiaoDich.MaLichSu)
            {
                return BadRequest();
            }

            // Kiểm tra giao dịch
            var giaoDich = await _context.GiaoDichs.FindAsync(lichSuGiaoDich.MaGiaoDich);
            if (giaoDich == null)
            {
                return BadRequest("Không tìm thấy giao dịch");
            }

            // Kiểm tra người dùng
            var khachHang = await _context.KhachHangs.FindAsync(lichSuGiaoDich.ThucHienBoi);
            if (khachHang == null)
            {
                return BadRequest("Không tìm thấy người dùng");
            }

            // Kiểm tra hành động
            if (!IsValidHanhDong(lichSuGiaoDich.HanhDong))
            {
                return BadRequest("Hành động không hợp lệ. Chỉ chấp nhận: TaoMoi, CapNhat, Xoa");
            }

            _context.Entry(lichSuGiaoDich).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LichSuGiaoDichExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/LichSuGiaoDich/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLichSuGiaoDich(int id)
        {
            var lichSuGiaoDich = await _context.LichSuGiaoDichs.FindAsync(id);
            if (lichSuGiaoDich == null)
            {
                return NotFound();
            }

            _context.LichSuGiaoDichs.Remove(lichSuGiaoDich);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/LichSuGiaoDich/giaodich/{maGiaoDich}
        [HttpDelete("giaodich/{maGiaoDich}")]
        public async Task<IActionResult> DeleteLichSuGiaoDichsByGiaoDich(int maGiaoDich)
        {
            var lichSuGiaoDichs = await _context.LichSuGiaoDichs
                .Where(l => l.MaGiaoDich == maGiaoDich)
                .ToListAsync();

            if (!lichSuGiaoDichs.Any())
            {
                return NotFound();
            }

            _context.LichSuGiaoDichs.RemoveRange(lichSuGiaoDichs);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Xóa lịch sử giao dịch thành công",
                soLichSuDaXoa = lichSuGiaoDichs.Count
            });
        }

        private bool LichSuGiaoDichExists(int id)
        {
            return _context.LichSuGiaoDichs.Any(e => e.MaLichSu == id);
        }

        private bool IsValidHanhDong(string hanhDong)
        {
            return new[] { "TaoMoi", "CapNhat", "Xoa" }.Contains(hanhDong);
        }
    }
}