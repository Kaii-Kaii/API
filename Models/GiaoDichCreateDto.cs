using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_ThuChi.Models
{
    public class GiaoDichCreateDto
    {
        [Required(ErrorMessage = "Mã người dùng không được để trống")]
        [StringLength(20, ErrorMessage = "Mã người dùng không được vượt quá 20 ký tự")]
        public string MaNguoiDung { get; set; }

        [Required(ErrorMessage = "Mã ví không được để trống")]
        public int MaVi { get; set; }

        public string? MAHANGMUC { get; set; }

        [Required(ErrorMessage = "Số tiền không được để trống")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SoTien { get; set; }

        [StringLength(255, ErrorMessage = "Ghi chú không được vượt quá 255 ký tự")]
        public string GhiChu { get; set; }

        [Required(ErrorMessage = "Ngày giao dịch không được để trống")]
        [Column(TypeName = "date")]
        public DateTime NgayGiaoDich { get; set; }

        [Required(ErrorMessage = "Loại giao dịch không được để trống")]
        [StringLength(50, ErrorMessage = "Loại giao dịch không được vượt quá 50 ký tự")]
        [RegularExpression("^(Thu|Chi|ChuyenKhoan)$", ErrorMessage = "Loại giao dịch phải là Thu, Chi hoặc ChuyenKhoan")]
        public string LoaiGiaoDich { get; set; }

        public int? MaViNhan { get; set; }
    }
}