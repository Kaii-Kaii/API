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

        [HttpGet("bykhachhang/{MaNguoiDung}")]
        public async Task<IActionResult> GetByKhachHang(string MaNguoiDung)
        {
            var list = await _context.HangMucs
                .Where(hm => hm.MaNguoiDung == MaNguoiDung)
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
        public async Task<IActionResult> UpdateHayDung(UpdateHayDungRequest model)
        {
            var hangMuc = await _context.HangMucs.FindAsync(model.MAHANGMUC, model.MaNguoiDung);
            if (hangMuc == null) return NotFound();

            hangMuc.HAYDUNG = model.HAYDUNG;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateHangMuc([FromBody] HangMuc model)
        {
            if (string.IsNullOrEmpty(model.MAHANGMUC) ||
                string.IsNullOrEmpty(model.MaNguoiDung) ||
                string.IsNullOrEmpty(model.TENHANGMUC) ||
                string.IsNullOrEmpty(model.LOAI))
            {
                return BadRequest("Thiếu thông tin bắt buộc.");
            }

            var hangMuc = await _context.HangMucs
                .FirstOrDefaultAsync(x => x.MAHANGMUC == model.MAHANGMUC && x.MaNguoiDung == model.MaNguoiDung);

            if (hangMuc == null)
                return NotFound("Không tìm thấy hạng mục.");

            // Kiểm tra trùng tên trong cùng loại (trừ chính nó)
            var isDuplicate = await _context.HangMucs.AnyAsync(x =>
                x.MaNguoiDung == model.MaNguoiDung &&
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
        [HttpGet("user/{maNguoiDung}")]
        public async Task<ActionResult<IEnumerable<HangMuc>>> GetHangMucsByUser(string maNguoiDung)
        {
            var list = await _context.HangMucs
                .Where(hm => hm.MaNguoiDung == maNguoiDung)
                .ToListAsync();
            return Ok(list);
        }

        [HttpGet("{maHangMuc}")]
        public async Task<IActionResult> GetById(string maHangMuc)
        {
            var hangMuc = await _context.HangMucs.FirstOrDefaultAsync(x => x.MAHANGMUC == maHangMuc);
            if (hangMuc == null)
                return NotFound("Không tìm thấy hạng mục.");
            return Ok(hangMuc);
        }

        /// <summary>
        /// Cập nhật trường sotienhientai của HangMuc bằng tổng SoTien của các GiaoDich cùng MaHangMuc
        /// </summary>
        [HttpPut("capnhat-sotienhientai/{maHangMuc}")]
        public async Task<IActionResult> CapNhatSoTienHienTai(string maHangMuc)
        {
            var hangMuc = await _context.HangMucs.FirstOrDefaultAsync(x => x.MAHANGMUC == maHangMuc);
            if (hangMuc == null)
                return NotFound("Không tìm thấy hạng mục.");

            // Chỉ cộng tổng các giao dịch loại "Chi"
            var tongSoTien = await _context.GiaoDichs
                .Where(gd => gd.MaHangMuc == maHangMuc && gd.LoaiGiaoDich == "Chi")
                .SumAsync(gd => (decimal?)gd.SoTien) ?? 0;

            hangMuc.sotienhientai = tongSoTien;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã cập nhật sotienhientai", sotienhientai = tongSoTien });
        }
        [HttpPut("capnhat-toida/{maHangMuc}")]
        public async Task<IActionResult> CapNhatToiDa(string maHangMuc, [FromBody] CapNhatToiDaDto model)
        {
            var hangMuc = await _context.HangMucs.FirstOrDefaultAsync(x => x.MAHANGMUC == maHangMuc);
            if (hangMuc == null)
                return NotFound("Không tìm thấy hạng mục.");

            hangMuc.toida = model.toida;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã cập nhật toida", toida = model.toida });
        }

    }
}