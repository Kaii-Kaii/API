using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QL_ThuChi.Data;
using QL_ThuChi.Models;

namespace QL_ThuChi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HangMucController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HangMucController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/HangMuc
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HangMuc>>> GetAll()
        {
            var list = await _context.HangMucs.ToListAsync();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HangMuc model)
        {
            if (model == null)
                return BadRequest("Dữ liệu không hợp lệ.");

            // Có thể thêm kiểm tra trùng tên hoặc các điều kiện khác ở đây nếu cần

            _context.HangMucs.Add(model);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Tạo hạng mục thành công", data = model });
        }

        [HttpGet("bykhachhang/{makh}")]
        public async Task<IActionResult> GetByKhachHang(string makh)
        {
            var list = await _context.HangMucs
                .Where(hm => hm.MAKH == makh)
                .ToListAsync();
            return Ok(list);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddHangMuc([FromBody] HangMuc model)
        {
            if (model == null) return BadRequest();

            // Sinh mã hạng mục tự động
            model.MAHANGMUC = GenerateHangMucCode();

            _context.HangMucs.Add(model);
            await _context.SaveChangesAsync();
            return Ok(model);
        }

        [HttpPut("updateHayDung")]
        public async Task<IActionResult> UpdateHayDung([FromBody] UpdateHayDungRequest model)
        {
            if (string.IsNullOrEmpty(model.MAHANGMUC))
                return BadRequest("MAHANGMUC is required.");

            var hangMuc = await _context.HangMucs.FindAsync(model.MAHANGMUC);
            if (hangMuc == null)
                return NotFound();

            hangMuc.HAYDUNG = model.HAYDUNG;
            await _context.SaveChangesAsync();
            return Ok(hangMuc);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateHangMuc([FromBody] HangMuc model)
        {
            if (string.IsNullOrEmpty(model.MAHANGMUC) ||
                string.IsNullOrEmpty(model.MAKH) ||
                string.IsNullOrEmpty(model.TENHANGMUC) ||
                string.IsNullOrEmpty(model.LOAI))
            {
                return BadRequest("Thiếu thông tin bắt buộc.");
            }

            var hangMuc = await _context.HangMucs
                .FirstOrDefaultAsync(x => x.MAHANGMUC == model.MAHANGMUC && x.MAKH == model.MAKH);

            if (hangMuc == null)
                return NotFound("Không tìm thấy hạng mục.");

            // Kiểm tra trùng tên trong cùng loại (trừ chính nó)
            var isDuplicate = await _context.HangMucs.AnyAsync(x =>
                x.MAKH == model.MAKH &&
                x.LOAI == model.LOAI &&
                x.TENHANGMUC.ToLower() == model.TENHANGMUC.Trim().ToLower() &&
                x.MAHANGMUC != model.MAHANGMUC
            );
            if (isDuplicate)
                return BadRequest("Tên hạng mục đã tồn tại trong mục này!");

            hangMuc.TENHANGMUC = model.TENHANGMUC.Trim();
            hangMuc.ICON = model.ICON ?? "";
            hangMuc.LOAI = model.LOAI;
            hangMuc.HAYDUNG = model.HAYDUNG;

            await _context.SaveChangesAsync();
            return Ok(hangMuc);
        }

        [HttpDelete("delete/{maHangMuc}")]
        public async Task<IActionResult> DeleteHangMuc(string maHangMuc)
        {
            var hangMuc = await _context.HangMucs.FirstOrDefaultAsync(x => x.MAHANGMUC == maHangMuc);
            if (hangMuc == null)
                return NotFound("Không tìm thấy hạng mục.");

            _context.HangMucs.Remove(hangMuc);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Xóa thành công!" });
        }
        private string GenerateHangMucCode()
        {
            var lastHangMuc = _context.HangMucs
                .OrderByDescending(t => t.MAHANGMUC)
                .FirstOrDefault();

            string lastCode = lastHangMuc?.MAHANGMUC ?? "HM0000";
            int number = int.Parse(lastCode.Substring(2)) + 1;
            return "HM" + number.ToString("D4");
        }
    }
}