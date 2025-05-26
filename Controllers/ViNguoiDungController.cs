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

        // POST: api/ViNguoiDung
        [HttpPost]
        public async Task<ActionResult<ViNguoiDung>> Create(ViNguoiDung viNguoiDung)
        {
            bool exists = await _context.ViNguoiDungs.AnyAsync(vnd =>
                vnd.MaNguoiDung == viNguoiDung.MaNguoiDung &&
                vnd.MaVi == viNguoiDung.MaVi &&
                vnd.TenTaiKhoan == viNguoiDung.TenTaiKhoan);

            if (exists)
                return Conflict("Ví người dùng đã tồn tại với khóa chính này.");

            _context.ViNguoiDungs.Add(viNguoiDung);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new
            {
                maNguoiDung = viNguoiDung.MaNguoiDung,
                maVi = viNguoiDung.MaVi,
                tenTaiKhoan = viNguoiDung.TenTaiKhoan
            }, viNguoiDung);
        }

        // PUT: api/ViNguoiDung/{maNguoiDung}/{maVi}/{tenTaiKhoan}
        [HttpPut("{maNguoiDung}/{maVi}/{tenTaiKhoan}")]
        public async Task<IActionResult> Update(string maNguoiDung, int maVi, string tenTaiKhoan, ViNguoiDung viNguoiDung)
        {
            if (maNguoiDung != viNguoiDung.MaNguoiDung || maVi != viNguoiDung.MaVi || tenTaiKhoan != viNguoiDung.TenTaiKhoan)
                return BadRequest("Khóa chính trong URL không khớp với dữ liệu gửi lên.");

            _context.Entry(viNguoiDung).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                bool exists = await _context.ViNguoiDungs.AnyAsync(vnd =>
                    vnd.MaNguoiDung == maNguoiDung &&
                    vnd.MaVi == maVi &&
                    vnd.TenTaiKhoan == tenTaiKhoan);

                if (!exists)
                    return NotFound();

                throw;
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
