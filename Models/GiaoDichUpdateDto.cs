using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_ThuChi.Models
{
    public class GiaoDichUpdateDto
    {
        public int? MaVi { get; set; }
        public int? MaDanhMucNguoiDung { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? SoTien { get; set; }

        [StringLength(255)]
        public string GhiChu { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayGiaoDich { get; set; }

        [StringLength(50)]
        [RegularExpression("^(Thu|Chi|ChuyenKhoan)$", ErrorMessage = "Loại giao dịch phải là Thu, Chi hoặc ChuyenKhoan")]
        public string LoaiGiaoDich { get; set; }
        public int? MaViNhan { get; set; }
    }
}