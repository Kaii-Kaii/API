using System;
using System.ComponentModel.DataAnnotations;

namespace QL_ThuChi.Models
{
    public class LichSuGiaoDichCreateDto
    {
        [Required]
        public int MaGiaoDich { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression("^(TaoMoi|CapNhat|Xoa)$", ErrorMessage = "HanhDong must be either 'TaoMoi', 'CapNhat', or 'Xoa'")]
        public string HanhDong { get; set; }

        public decimal? SoTienCu { get; set; }

        public decimal? SoTienMoi { get; set; }

        [Required]
        [StringLength(20)]
        public string ThucHienBoi { get; set; }

        public DateTime ThoiGian { get; set; }
    }
}