using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QL_ThuChi.Data;
using QL_ThuChi.Models;

namespace QL_ThuChi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ViController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Vi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vi>>> GetAll()
        {
            return await _context.Vi.ToListAsync();
        }

        // GET: api/Vi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vi>> GetById(int id)
        {
            var vi = await _context.Vi.FindAsync(id);

            if (vi == null)
            {
                return NotFound();
            }

            return vi;
        }
    }
}
