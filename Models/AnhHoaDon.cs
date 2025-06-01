using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_ThuChi.Models
{
    public class AnhHoaDon
    {
        [Key]
        public int MaAnh { get; set; }

        [Required]
        public int MaGiaoDich { get; set; }

        [Required]
        [StringLength(255)]
        public string DuongDanAnh { get; set; }

        public DateTime NgayTaiLen { get; set; }

        // Navigation property
        [ForeignKey("MaGiaoDich")]
        public GiaoDich GiaoDich { get; set; }
    }
}