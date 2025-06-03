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
    public class DanhMucNguoiDungController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DanhMucNguoiDungController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/DanhMucNguoiDung
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DanhMucNguoiDung>>> GetDanhMucNguoiDungs()
        {
            return await _context.DanhMucNguoiDungs
                .Include(d => d.KhachHang)
                .ToListAsync();
        }

        // GET: api/DanhMucNguoiDung/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DanhMucNguoiDung>> GetDanhMucNguoiDung(int id)
        {
            var danhMucNguoiDung = await _context.DanhMucNguoiDungs
                .Include(d => d.KhachHang)
                .FirstOrDefaultAsync(d => d.MaDanhMucNguoiDung == id);

            if (danhMucNguoiDung == null)
            {
                return NotFound();
            }

            return danhMucNguoiDung;
        }

        // GET: api/DanhMucNguoiDung/user/{maNguoiDung}
        [HttpGet("user/{maNguoiDung}")]
        public async Task<ActionResult<IEnumerable<DanhMucNguoiDung>>> GetDanhMucNguoiDungsByUser(string maNguoiDung)
        {
            return await _context.DanhMucNguoiDungs
                .Include(d => d.KhachHang)
                .Where(d => d.MaNguoiDung == maNguoiDung)
                .ToListAsync();
        }

        // POST: api/DanhMucNguoiDung
        [HttpPost]
        public async Task<ActionResult<DanhMucNguoiDung>> CreateDanhMucNguoiDung(DanhMucNguoiDung danhMucNguoiDung)
        {
            // Kiểm tra xem người dùng có tồn tại không
            var khachHang = await _context.KhachHangs.FindAsync(danhMucNguoiDung.MaNguoiDung);
            if (khachHang == null)
            {
                return BadRequest("Không tìm thấy người dùng");
            }

            _context.DanhMucNguoiDungs.Add(danhMucNguoiDung);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDanhMucNguoiDung), new { id = danhMucNguoiDung.MaDanhMucNguoiDung }, danhMucNguoiDung);
        }

        // PUT: api/DanhMucNguoiDung/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDanhMucNguoiDung(int id, DanhMucNguoiDung danhMucNguoiDung)
        {
            if (id != danhMucNguoiDung.MaDanhMucNguoiDung)
            {
                return BadRequest();
            }

            // Kiểm tra xem người dùng có tồn tại không
            var khachHang = await _context.KhachHangs.FindAsync(danhMucNguoiDung.MaNguoiDung);
            if (khachHang == null)
            {
                return BadRequest("Không tìm thấy người dùng");
            }

            _context.Entry(danhMucNguoiDung).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DanhMucNguoiDungExists(id))
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

        // DELETE: api/DanhMucNguoiDung/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDanhMucNguoiDung(int id)
        {
            var danhMucNguoiDung = await _context.DanhMucNguoiDungs.FindAsync(id);
            if (danhMucNguoiDung == null)
            {
                return NotFound();
            }

            // Kiểm tra xem có giao dịch nào đang sử dụng danh mục này không
            var hasGiaoDichs = await _context.GiaoDichs
                .AnyAsync(g => g.MaDanhMucNguoiDung == id);

            if (hasGiaoDichs)
            {
                return BadRequest("Không thể xóa danh mục này vì đang được sử dụng trong giao dịch");
            }

            _context.DanhMucNguoiDungs.Remove(danhMucNguoiDung);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST 

        private bool DanhMucNguoiDungExists(int id)
        {
            return _context.DanhMucNguoiDungs.Any(e => e.MaDanhMucNguoiDung == id);
        }
    }
}