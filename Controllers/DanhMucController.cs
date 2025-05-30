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
    public class DanhMucController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DanhMucController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/DanhMuc
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DanhMuc>>> GetDanhMucs()
        {
            return await _context.DanhMucs
                .Include(d => d.Loai)
                .ToListAsync();
        }

        // GET: api/DanhMuc/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DanhMuc>> GetDanhMuc(int id)
        {
            var danhMuc = await _context.DanhMucs
                .Include(d => d.Loai)
                .FirstOrDefaultAsync(d => d.MaDanhMuc == id);

            if (danhMuc == null)
            {
                return NotFound();
            }

            return danhMuc;
        }

        // GET: api/DanhMuc/loai/{maLoaiDanhMuc}
        [HttpGet("loai/{maLoaiDanhMuc}")]
        public async Task<ActionResult<IEnumerable<DanhMuc>>> GetDanhMucsByLoai(int maLoaiDanhMuc)
        {
            return await _context.DanhMucs
                .Include(d => d.Loai)
                .Where(d => d.MaLoaiDanhMuc == maLoaiDanhMuc)
                .ToListAsync();
        }

        // POST: api/DanhMuc
        [HttpPost]
        public async Task<ActionResult<DanhMuc>> CreateDanhMuc(DanhMuc danhMuc)
        {
            var loai = await _context.Loais.FindAsync(danhMuc.MaLoaiDanhMuc);
            if (loai == null)
            {
                return BadRequest("Không tìm thấy loại");
            }

            _context.DanhMucs.Add(danhMuc);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDanhMuc), new { id = danhMuc.MaDanhMuc }, danhMuc);
        }

        // PUT: api/DanhMuc/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDanhMuc(int id, DanhMuc danhMuc)
        {
            if (id != danhMuc.MaDanhMuc)
            {
                return BadRequest();
            }

            var loai = await _context.Loais.FindAsync(danhMuc.MaLoaiDanhMuc);
            if (loai == null)
            {
                return BadRequest("Không tìm thấy loại");
            }

            _context.Entry(danhMuc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DanhMucExists(id))
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

        // DELETE: api/DanhMuc/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDanhMuc(int id)
        {
            var danhMuc = await _context.DanhMucs.FindAsync(id);
            if (danhMuc == null)
            {
                return NotFound();
            }

            // Kiểm tra xem có danh mục người dùng nào đang sử dụng danh mục này không
            var hasDanhMucNguoiDungs = await _context.DanhMucNguoiDungs
                .AnyAsync(d => d.TenDanhMucNguoiDung == danhMuc.TenDanhMuc);

            if (hasDanhMucNguoiDungs)
            {
                return BadRequest("Không thể xóa danh mục này vì đang được sử dụng bởi người dùng");
            }

            _context.DanhMucs.Remove(danhMuc);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DanhMucExists(int id)
        {
            return _context.DanhMucs.Any(e => e.MaDanhMuc == id);
        }
    }
}