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
            return await _context.ViNguoiDungs
                .Include(vnd => vnd.Vi)
                .Include(vnd => vnd.LoaiTien)
                .Include(vnd => vnd.KhachHang)
                .ToListAsync();
        }

        // GET: api/ViNguoiDung/{MaNguoiDung}/{MaVi}/{TenTaiKhoan}
        [HttpGet("{maNguoiDung}/{maVi}/{tenTaiKhoan}")]
        public async Task<ActionResult<ViNguoiDung>> GetById(string maNguoiDung, int maVi, string tenTaiKhoan)
        {
            var viNguoiDung = await _context.ViNguoiDungs
                .Include(vnd => vnd.Vi)
                .Include(vnd => vnd.LoaiTien)
                .Include(vnd => vnd.KhachHang)
                .FirstOrDefaultAsync(vnd => vnd.MaNguoiDung == maNguoiDung
                                        && vnd.MaVi == maVi
                                        && vnd.TenTaiKhoan == tenTaiKhoan);

            if (viNguoiDung == null)
            {
                return NotFound();
            }

            return viNguoiDung;
        }

        // POST: api/ViNguoiDung
        [HttpPost]
        public async Task<ActionResult<ViNguoiDung>> Create(ViNguoiDung viNguoiDung)
        {
            _context.ViNguoiDungs.Add(viNguoiDung);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new
            {
                maNguoiDung = viNguoiDung.MaNguoiDung,
                maVi = viNguoiDung.MaVi,
                tenTaiKhoan = viNguoiDung.TenTaiKhoan
            }, viNguoiDung);
        }

        // PUT: api/ViNguoiDung/{MaNguoiDung}/{MaVi}/{TenTaiKhoan}
        [HttpPut("{maNguoiDung}/{maVi}/{tenTaiKhoan}")]
        public async Task<IActionResult> Update(string maNguoiDung, int maVi, string tenTaiKhoan, ViNguoiDung viNguoiDung)
        {
            if (maNguoiDung != viNguoiDung.MaNguoiDung || maVi != viNguoiDung.MaVi || tenTaiKhoan != viNguoiDung.TenTaiKhoan)
            {
                return BadRequest();
            }

            _context.Entry(viNguoiDung).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                bool exists = _context.ViNguoiDungs.Any(vnd =>
                    vnd.MaNguoiDung == maNguoiDung && vnd.MaVi == maVi && vnd.TenTaiKhoan == tenTaiKhoan);
                if (!exists)
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/ViNguoiDung/{MaNguoiDung}/{MaVi}/{TenTaiKhoan}
        [HttpDelete("{maNguoiDung}/{maVi}/{tenTaiKhoan}")]
        public async Task<IActionResult> Delete(string maNguoiDung, int maVi, string tenTaiKhoan)
        {
            var viNguoiDung = await _context.ViNguoiDungs.FindAsync(maNguoiDung, maVi, tenTaiKhoan);
            if (viNguoiDung == null)
            {
                return NotFound();
            }

            _context.ViNguoiDungs.Remove(viNguoiDung);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
