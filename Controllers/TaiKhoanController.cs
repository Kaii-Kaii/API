using Microsoft.AspNetCore.Mvc;
using QL_ThuChi.Data;
using QL_ThuChi.Models;
using Microsoft.EntityFrameworkCore;

namespace QL_ThuChi.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class TaiKhoanController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaiKhoanController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaiKhoan>>> GetAll()
        {
            return await _context.TaiKhoans.ToListAsync();
        }


        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] TaiKhoan model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Dữ liệu không hợp lệ");
                }

                // Tạo mã tài khoản tự động nếu không có
                if (string.IsNullOrEmpty(model.MATAIKHOAN))
                {
                    model.MATAIKHOAN = GenerateAccountCode();
                }

                // Kiểm tra nếu tài khoản đã tồn tại
                var existingUser = await _context.TaiKhoans
                    .FirstOrDefaultAsync(u => u.TENDANGNHAP == model.TENDANGNHAP);

                if (existingUser != null)
                {
                    return BadRequest("Tên đăng nhập đã tồn tại");
                }

                _context.TaiKhoans.Add(model);
                await _context.SaveChangesAsync();

                return Ok("Tạo tài khoản thành công");
            }
            catch (Exception ex)
            {
                // Trả về mã lỗi 500 kèm chi tiết lỗi để debug (không nên dùng chi tiết khi đưa vào production)
                return StatusCode(500, new
                {
                    message = "Đã xảy ra lỗi phía server.",
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        private string GenerateAccountCode()
        {
            var lastAccount = _context.TaiKhoans.OrderByDescending(t => t.MATAIKHOAN).FirstOrDefault();
            string lastCode = lastAccount?.MATAIKHOAN ?? "TK0000"; // Mặc định "TK0000" nếu không có tài khoản
            int number = int.Parse(lastCode.Substring(2)) + 1; // Tăng dần số
            return "TK" + number.ToString("D4"); // Mã tài khoản mới
        }
    }

}
