using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QL_ThuChi.Data;
using QL_ThuChi.Models;

namespace QL_ThuChi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiTienController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LoaiTienController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/LoaiTien
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoaiTien>>> GetAll()
        {
            return await _context.LoaiTiens.ToListAsync();
        }

        // GET: api/LoaiTien/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoaiTien>> GetById(int id)
        {
            var loaiTien = await _context.LoaiTiens.FindAsync(id);

            if (loaiTien == null)
            {
                return NotFound();
            }

            return loaiTien;
        }

        // POST: api/LoaiTien
        [HttpPost]
        public async Task<ActionResult<LoaiTien>> Create(LoaiTien loaiTien)
        {
            _context.LoaiTiens.Add(loaiTien);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = loaiTien.MaLoai }, loaiTien);
        }

        // PUT: api/LoaiTien/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, LoaiTien loaiTien)
        {
            if (id != loaiTien.MaLoai)
            {
                return BadRequest();
            }

            _context.Entry(loaiTien).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.LoaiTiens.Any(e => e.MaLoai == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/LoaiTien/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var loaiTien = await _context.LoaiTiens.FindAsync(id);
            if (loaiTien == null)
            {
                return NotFound();
            }

            _context.LoaiTiens.Remove(loaiTien);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
