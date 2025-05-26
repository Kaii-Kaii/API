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
    }
}
