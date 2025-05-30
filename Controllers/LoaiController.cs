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
    public class LoaiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LoaiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Loai
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Loai>>> GetLoais()
        {
            return await _context.Loais.ToListAsync();
        }

        // GET: api/Loai/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Loai>> GetLoai(int id)
        {
            var loai = await _context.Loais.FindAsync(id);

            if (loai == null)
            {
                return NotFound();
            }

            return loai;
        }

        // POST: api/Loai
        [HttpPost]
        public async Task<ActionResult<Loai>> CreateLoai(Loai loai)
        {
            _context.Loais.Add(loai);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLoai), new { id = loai.MaLoai }, loai);
        }

        // PUT: api/Loai/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLoai(int id, Loai loai)
        {
            if (id != loai.MaLoai)
            {
                return BadRequest();
            }

            _context.Entry(loai).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoaiExists(id))
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

        // DELETE: api/Loai/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoai(int id)
        {
            var loai = await _context.Loais.FindAsync(id);
            if (loai == null)
            {
                return NotFound();
            }

            // Kiểm tra xem có danh mục nào đang sử dụng loại này không
            var hasDanhMucs = await _context.DanhMucs.AnyAsync(d => d.MaLoaiDanhMuc == id);
            if (hasDanhMucs)
            {
                return BadRequest("Không thể xóa loại này vì đang được sử dụng bởi danh mục");
            }

            _context.Loais.Remove(loai);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoaiExists(int id)
        {
            return _context.Loais.Any(e => e.MaLoai == id);
        }
    }
}