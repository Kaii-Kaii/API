using Microsoft.AspNetCore.Mvc;
using QL_ThuChi.Data;
using QL_ThuChi.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;

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

        [HttpGet("CheckUsername")]
        public async Task<IActionResult> CheckUsername([FromQuery] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest("Thiếu tên đăng nhập.");

            var exists = await _context.TaiKhoans.AnyAsync(u => u.TENDANGNHAP == username);
            // Trả về true nếu đã tồn tại, false nếu chưa
            return Ok(exists);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] TaiKhoan model)
        {
            try
            {
                if (model == null ||
                    string.IsNullOrWhiteSpace(model.TENDANGNHAP) ||
                    string.IsNullOrWhiteSpace(model.MATKHAU) ||
                    string.IsNullOrWhiteSpace(model.EMAIL))
                {
                    return BadRequest("Thông tin tài khoản không hợp lệ.");
                }

                // Kiểm tra tên đăng nhập đã tồn tại
                var existingUser = await _context.TaiKhoans
                    .FirstOrDefaultAsync(u => u.TENDANGNHAP == model.TENDANGNHAP);
                if (existingUser != null)
                {
                    return BadRequest("Tên đăng nhập đã tồn tại.");
                }

                // Tạo mã tài khoản tự động
                model.MATAIKHOAN = GenerateAccountCode();

                // Sinh token xác thực email
                var tokenBytes = new byte[32];
                using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
                {
                    rng.GetBytes(tokenBytes);
                }
                string emailToken = Convert.ToBase64String(tokenBytes);

                // Thiết lập mặc định
                model.ISEMAILCONFIRMED = 0;
                model.EMAILCONFIRMATIONTOKEN = emailToken;
                model.OTP = null;
                model.KhachHang = null;

                _context.TaiKhoans.Add(model);
                await _context.SaveChangesAsync();

                // Gửi email xác thực
                string confirmationLink = $"https://localhost:7283/api/TaiKhoan/ConfirmEmail?token={Uri.EscapeDataString(emailToken)}";
                await SendConfirmationEmail(model.EMAIL, confirmationLink);

                return Ok(new { message = "Tạo tài khoản thành công. Vui lòng kiểm tra email để xác thực." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Đã xảy ra lỗi phía server.",
                    error = ex.Message
                });
            }
        }
        private async Task SendConfirmationEmail(string toEmail, string confirmationLink)
        {
            var smtp = new SmtpClient("smtp.gmail.com") // Thêm host ở đây
            {
                Credentials = new NetworkCredential("chaytue0203@gmail.com", "kctw ltds teaj luvb"),
                EnableSsl = true,
                Port = 587
            };
            var mail = new MailMessage("khangtuong040@gmail.com", toEmail)
            {
                Subject = "Xác thực tài khoản",
                Body = $"Vui lòng xác thực tài khoản bằng cách click vào link: {confirmationLink}"
            };
            await smtp.SendMailAsync(mail);
        }
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
        {
            if (string.IsNullOrEmpty(token))
                return BadRequest("Token không hợp lệ.");

            var user = await _context.TaiKhoans.FirstOrDefaultAsync(u => u.EMAILCONFIRMATIONTOKEN == token);
            if (user == null)
                return NotFound("Token không hợp lệ hoặc đã được xác thực.");

            user.ISEMAILCONFIRMED = 1;
            user.EMAILCONFIRMATIONTOKEN = null;
            await _context.SaveChangesAsync();

            return Ok("Xác thực email thành công! Bạn có thể đăng nhập.");
        }
        private string GenerateAccountCode()
        {
            var lastAccount = _context.TaiKhoans
                .OrderByDescending(t => t.MATAIKHOAN)
                .FirstOrDefault();

            string lastCode = lastAccount?.MATAIKHOAN ?? "TK0000";
            int number = int.Parse(lastCode.Substring(2)) + 1;
            return "TK" + number.ToString("D4");
        }

        // Gửi OTP về email khi quên mật khẩu
        [HttpPost("SendOtp")]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpRequest data)
        {
            string username = data?.Username;
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest("Thiếu tên đăng nhập.");

            var user = await _context.TaiKhoans.FirstOrDefaultAsync(u => u.TENDANGNHAP == username);
            if (user == null)
                return NotFound("Không tìm thấy tài khoản.");

            // Sinh OTP ngẫu nhiên 6 số
            var rng = new Random();
            int otp = rng.Next(100000, 999999);

            user.OTP = otp;
            await _context.SaveChangesAsync();

            // Gửi OTP về email
            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Credentials = new NetworkCredential("chaytue0203@gmail.com", "kctw ltds teaj luvb"),
                EnableSsl = true,
                Port = 587
            };
            var mail = new MailMessage("khangtuong040@gmail.com", user.EMAIL)
            {
                Subject = "OTP đặt lại mật khẩu",
                Body = $"Mã OTP đặt lại mật khẩu của bạn là: {otp}"
            };
            await smtp.SendMailAsync(mail);

            return Ok(new { message = "OTP đã được gửi về email." });
        }



        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest data)
        {
            string username = data?.Username;
            string newPassword = data?.NewPassword;
            int otp = data.Otp;

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(newPassword) ||
                otp == 0)
            {
                return BadRequest("Thiếu thông tin.");
            }

            var user = await _context.TaiKhoans.FirstOrDefaultAsync(u => u.TENDANGNHAP == username);
            if (user == null)
                return NotFound("Không tìm thấy tài khoản.");

            if (user.OTP != otp)
                return BadRequest("OTP không đúng hoặc đã hết hạn.");

            user.MATKHAU = newPassword;
            user.OTP = null;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đổi mật khẩu thành công." });
        }
    }
}
