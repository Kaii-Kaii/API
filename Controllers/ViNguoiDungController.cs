using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QL_ThuChi.Data;
using QL_ThuChi.Models;

namespace QL_ThuChi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViNguoiDungController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ViNguoiDungController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ViNguoiDung
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ViNguoiDung>>> GetAll()
        {
            var list = await _context.ViNguoiDungs
                .Include(vnd => vnd.Vi)
                .Include(vnd => vnd.LoaiTien)
                .Include(vnd => vnd.KhachHang)
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/ViNguoiDung/{maNguoiDung}/{maVi}/{tenTaiKhoan}
        [HttpGet("{maNguoiDung}/{maVi}/{tenTaiKhoan}")]
        public async Task<ActionResult<ViNguoiDung>> GetById(string maNguoiDung, int maVi, string tenTaiKhoan)
        {
            var viNguoiDung = await _context.ViNguoiDungs
                .Include(vnd => vnd.Vi)
                .Include(vnd => vnd.LoaiTien)
                .Include(vnd => vnd.KhachHang)
                .FirstOrDefaultAsync(vnd =>
                    vnd.MaNguoiDung == maNguoiDung &&
                    vnd.MaVi == maVi &&
                    vnd.TenTaiKhoan == tenTaiKhoan);

            if (viNguoiDung == null)
                return NotFound();

            return Ok(viNguoiDung);
        }


        // PUT: api/ViNguoiDung/{maNguoiDung}/{maVi}/{tenTaiKhoan}
        [HttpPut("{maNguoiDung}/{maVi}/{tenTaiKhoan}")]
        public async Task<IActionResult> CapNhatViNguoiDung(
        string maNguoiDung, int maVi, string tenTaiKhoan,
        [FromBody] ViNguoiDungUpdateDto dto)
            {
                // Tìm bản ghi cũ theo khóa chính
                var viCu = await _context.ViNguoiDungs
                    .FirstOrDefaultAsync(v =>
                        v.MaNguoiDung.Trim() == maNguoiDung.Trim() &&
                        v.MaVi == maVi &&
                        v.TenTaiKhoan == tenTaiKhoan);

                if (viCu == null)
                    return NotFound("Ví không tồn tại.");

                // Xóa bản ghi cũ
                _context.ViNguoiDungs.Remove(viCu);

                // Tạo bản ghi mới với dữ liệu cập nhật, đặc biệt TenTaiKhoan mới
                var viMoi = new ViNguoiDung
                {
                    MaNguoiDung = viCu.MaNguoiDung,
                    MaVi = viCu.MaVi,
                    MaLoaiTien = dto.MaLoaiTien,
                    TenTaiKhoan = dto.TenTaiKhoan,
                    DienGiai = dto.DienGiai,
                    SoDu = dto.SoDu
                };

                // Thêm bản ghi mới
                _context.ViNguoiDungs.Add(viMoi);

                // Lưu thay đổi
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    // Xử lý lỗi nếu có (ví dụ trùng khóa chính)
                    return BadRequest(new { message = ex.Message });
                }

                return NoContent();
            }

        // DELETE: api/ViNguoiDung/{maNguoiDung}/{maVi}/{tenTaiKhoan}
        [HttpDelete("{maNguoiDung}/{maVi}/{tenTaiKhoan}")]
        public async Task<IActionResult> Delete(string maNguoiDung, int maVi, string tenTaiKhoan)
        {
            var viNguoiDung = await _context.ViNguoiDungs
                .FirstOrDefaultAsync(vnd =>
                    vnd.MaNguoiDung == maNguoiDung &&
                    vnd.MaVi == maVi &&
                    vnd.TenTaiKhoan == tenTaiKhoan);

            if (viNguoiDung == null)
                return NotFound();

            _context.ViNguoiDungs.Remove(viNguoiDung);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
