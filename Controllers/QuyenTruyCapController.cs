using Microsoft.AspNetCore.Mvc;
using QL_ThuChi.Data;
using QL_ThuChi.Models;
using Microsoft.EntityFrameworkCore;

namespace QL_ThuChi.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class QuyenTruyCapController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QuyenTruyCapController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuyenTruyCap>>> GetAll()
        {
            return await _context.QuyenTruyCaps.ToListAsync();
        }
    }

}
