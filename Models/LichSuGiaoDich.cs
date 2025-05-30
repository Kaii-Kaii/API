using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_ThuChi.Models
{
    public class LichSuGiaoDich
    {
        [Key]
        public int MaLichSu { get; set; }

        [Required]
        public int MaGiaoDich { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression("^(TaoMoi|CapNhat|Xoa)$", ErrorMessage = "HanhDong must be either 'TaoMoi', 'CapNhat', or 'Xoa'")]
        public string HanhDong { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SoTienCu { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SoTienMoi { get; set; }

        [Required]
        [StringLength(20)]
        public string ThucHienBoi { get; set; }

        public DateTime ThoiGian { get; set; }

        // Navigation properties
        [ForeignKey("MaGiaoDich")]
        public GiaoDich GiaoDich { get; set; }

        [ForeignKey("ThucHienBoi")]
        public KhachHang KhachHang { get; set; }
    }
}