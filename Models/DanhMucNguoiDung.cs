using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_ThuChi.Models
{
    public class DanhMucNguoiDung
    {
        [Key]
        public int MaDanhMucNguoiDung { get; set; }

        [Required]
        [StringLength(20)]
        public string MaNguoiDung { get; set; }

        [Required]
        [StringLength(100)]
        public string TenDanhMucNguoiDung { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ToiDa { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SoTienHienTai { get; set; }

        // Navigation properties
        [ForeignKey("MaNguoiDung")]
        public KhachHang KhachHang { get; set; }
    }
}