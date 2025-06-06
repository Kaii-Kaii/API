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



        [HttpPost]
        public async Task<ActionResult<KhachHang>> CreateKhachHang(ThongTinKhachHang dto)
        {
            var newMaKH = GenerateAccountCode();

            var kh = new KhachHang
            {
                MAKH = newMaKH,
                MATAIKHOAN = dto.MATAIKHOAN,
                HOTEN = dto.HOTEN,
                NGAYSINH = dto.NGAYSINH,
                SODT = dto.SODT,
                XU = 0,
                AVATAR = null
            };

            _context.KhachHangs.Add(kh);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = kh.MAKH }, kh);
        }

        // Hàm tạo mã khách hàng tự động
        private string GenerateAccountCode()
        {
            var lastAccount = _context.KhachHangs
                .OrderByDescending(t => t.MAKH)
                .FirstOrDefault();

            string lastCode = lastAccount?.MAKH ?? "KH0000";
            int number = int.Parse(lastCode.Substring(2)) + 1;
            return "KH" + number.ToString("D4");
        }
        [HttpPut("UpdateThongTin/{maKH}")]
        public async Task<IActionResult> UpdateThongTin(string maKH, [FromBody] KhachHangUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(maKH) || dto == null)
                return BadRequest("Thông tin không hợp lệ.");

            var khachHang = await _context.KhachHangs.FirstOrDefaultAsync(kh => kh.MAKH == maKH);
            if (khachHang == null)
                return NotFound("Không tìm thấy khách hàng.");

            // Cập nhật các thông tin cho phép thay đổi
            khachHang.HOTEN = dto.HOTEN ?? khachHang.HOTEN;
            khachHang.NGAYSINH = dto.NGAYSINH ?? khachHang.NGAYSINH;
            khachHang.SODT = dto.SODT ?? khachHang.SODT;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật thông tin khách hàng thành công." });
        }


    }

}
