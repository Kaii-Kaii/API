using Microsoft.AspNetCore.Mvc;
using QL_ThuChi.Data;
using QL_ThuChi.Models;
using Microsoft.EntityFrameworkCore;

namespace QL_ThuChi.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class KhachHangController : ControllerBase
    {
        private readonly AppDbContext _context;

        public KhachHangController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KhachHang>>> GetAll()
        {
            return await _context.KhachHangs.ToListAsync();
        }
    }

}
