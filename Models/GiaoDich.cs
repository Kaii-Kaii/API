using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_ThuChi.Models
{
    public class GiaoDich
    {
        [Key]
        public int MaGiaoDich { get; set; }

        [Required]
        [StringLength(20)]
        public string MaNguoiDung { get; set; }

        [Required]
        public int MaVi { get; set; }

        public int? MaDanhMucNguoiDung { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SoTien { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SoTienCu { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SoTienMoi { get; set; }

        [StringLength(255)]
        public string GhiChu { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime NgayGiaoDich { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression("^(Thu|Chi|ChuyenKhoan)$", ErrorMessage = "LoaiGiaoDich must be either 'Thu', 'Chi', or 'ChuyenKhoan'")]
        public string LoaiGiaoDich { get; set; }

        public int? MaViNhan { get; set; }

        // Navigation properties
        [ForeignKey("MaNguoiDung")]
        public KhachHang KhachHang { get; set; }

        [ForeignKey("MaVi")]
        public Vi Vi { get; set; }

        [ForeignKey("MaDanhMucNguoiDung, MaNguoiDung")]
        public DanhMucNguoiDung DanhMucNguoiDung { get; set; }

        [ForeignKey("MaViNhan")]
        public Vi ViNhan { get; set; }
    }
}