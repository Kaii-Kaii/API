using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QL_ThuChi.Data;
using QL_ThuChi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        // GET: api/ViNguoiDung/khachhang/{maKH}
        [HttpGet("khachhang/{maKH}")]
        public async Task<ActionResult<IEnumerable<ViNguoiDung>>> GetViByMaKH(string maKH)
        {
            var viNguoiDung = await _context.ViNguoiDungs
                .Include(v => v.Vi)
                .Include(v => v.LoaiTien)
                .Where(v => v.MaNguoiDung == maKH)
                .ToListAsync();

            if (viNguoiDung == null || !viNguoiDung.Any())
            {
                return NotFound($"Không tìm thấy ví nào cho khách hàng có mã {maKH}");
            }

            return viNguoiDung;
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ViNguoiDungCreateDto dto)
        {
            var exists = await _context.ViNguoiDungs.AnyAsync(v =>
                v.MaNguoiDung == dto.MaNguoiDung &&
                v.MaVi == dto.MaVi &&
                v.TenTaiKhoan == dto.TenTaiKhoan);

            if (exists)
                return BadRequest(new { message = "Ví đã tồn tại." });

            var model = new ViNguoiDung
            {
                MaNguoiDung = dto.MaNguoiDung,
                MaVi = dto.MaVi,
                TenTaiKhoan = dto.TenTaiKhoan,
                MaLoaiTien = dto.MaLoaiTien,
                DienGiai = dto.DienGiai,
                SoDu = dto.SoDu
            };

            _context.ViNguoiDungs.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new
            {
                maNguoiDung = model.MaNguoiDung,
                maVi = model.MaVi,
                tenTaiKhoan = model.TenTaiKhoan
            }, model);
        }
    }
}
