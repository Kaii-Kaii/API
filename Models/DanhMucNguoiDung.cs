using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

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

        //ALTER TABLE DanhMucNguoiDung
        //ADD ThuChi NVARCHAR(10);  -- hoặc CHAR(3) nếu chỉ dùng 'Thu'/'Chi'
        [Required]
        [StringLength(10)]
        public string ThuChi { get; set; } // 'Thu' hoặc 'Chi'

        // Navigation properties
        [ForeignKey("MaNguoiDung")]
        public KhachHang KhachHang { get; set; }
    }
}