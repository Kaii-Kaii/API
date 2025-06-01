using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QL_ThuChi.Data;
using QL_ThuChi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QL_ThuChi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnhHoaDonController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AnhHoaDonController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/AnhHoaDon
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnhHoaDon>>> GetAnhHoaDons()
        {
            return await _context.AnhHoaDons
                .Include(a => a.GiaoDich)
                .ToListAsync();
        }

        // GET: api/AnhHoaDon/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnhHoaDon>> GetAnhHoaDon(int id)
        {
            var anhHoaDon = await _context.AnhHoaDons
                .Include(a => a.GiaoDich)
                .FirstOrDefaultAsync(a => a.MaAnh == id);

            if (anhHoaDon == null)
            {
                return NotFound();
            }

            return anhHoaDon;
        }

        // GET: api/AnhHoaDon/giaodich/{maGiaoDich}
        [HttpGet("giaodich/{maGiaoDich}")]
        public async Task<ActionResult<IEnumerable<AnhHoaDon>>> GetAnhHoaDonsByGiaoDich(int maGiaoDich)
        {
            return await _context.AnhHoaDons
                .Include(a => a.GiaoDich)
                .Where(a => a.MaGiaoDich == maGiaoDich)
                .ToListAsync();
        }

        // POST: api/AnhHoaDon
        [HttpPost]
        public async Task<ActionResult<AnhHoaDonDto>> CreateAnhHoaDon(AnhHoaDonDto dto)
        {
            // Kiểm tra giao dịch tồn tại
            var giaoDich = await _context.GiaoDichs.FindAsync(dto.MaGiaoDich);
            if (giaoDich == null)
            {
                return BadRequest("Không tìm thấy giao dịch");
            }

            var anhHoaDon = new AnhHoaDon
            {
                MaGiaoDich = dto.MaGiaoDich,
                DuongDanAnh = dto.DuongDanAnh,
                NgayTaiLen = DateTime.Now
            };

            _context.AnhHoaDons.Add(anhHoaDon);
            await _context.SaveChangesAsync();

            return Ok(new AnhHoaDonDto
            {
                MaGiaoDich = anhHoaDon.MaGiaoDich,
                DuongDanAnh = anhHoaDon.DuongDanAnh
            });
        }

        // PUT: api/AnhHoaDon/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnhHoaDon(int id, AnhHoaDon anhHoaDon)
        {
            if (id != anhHoaDon.MaAnh)
            {
                return BadRequest();
            }

            // Validate GiaoDich exists
            var giaoDich = await _context.GiaoDichs.FindAsync(anhHoaDon.MaGiaoDich);
            if (giaoDich == null)
            {
                return BadRequest("Không tìm thấy giao dịch");
            }

            _context.Entry(anhHoaDon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnhHoaDonExists(id))
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

        // DELETE: api/AnhHoaDon/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnhHoaDon(int id)
        {
            var anhHoaDon = await _context.AnhHoaDons.FindAsync(id);
            if (anhHoaDon == null)
            {
                return NotFound();
            }

            _context.AnhHoaDons.Remove(anhHoaDon);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/AnhHoaDon/giaodich/{maGiaoDich}
        [HttpDelete("giaodich/{maGiaoDich}")]
        public async Task<IActionResult> DeleteAnhHoaDonsByGiaoDich(int maGiaoDich)
        {
            var anhHoaDons = await _context.AnhHoaDons
                .Where(a => a.MaGiaoDich == maGiaoDich)
                .ToListAsync();

            if (!anhHoaDons.Any())
            {
                return NotFound();
            }

            _context.AnhHoaDons.RemoveRange(anhHoaDons);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Xóa ảnh hóa đơn thành công",
                soAnhDaXoa = anhHoaDons.Count
            });
        }

        private bool AnhHoaDonExists(int id)
        {
            return _context.AnhHoaDons.Any(e => e.MaAnh == id);
        }
    }
}