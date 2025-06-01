using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_ThuChi.Models
{
    [Table("AnhHoaDon")]
    public class AnhHoaDon
    {
        [Key]
        [Column("MaAnh")]
        public int MaAnh { get; set; }

        [Required]
        [Column("MaGiaoDich")]
        public int MaGiaoDich { get; set; }

        [Required]
        [StringLength(255)]
        [Column("DuongDanAnh")]
        public string DuongDanAnh { get; set; }

        [Column("NgayTaiLen")]
        public DateTime NgayTaiLen { get; set; } = DateTime.Now;

        // Navigation property
        [ForeignKey("MaGiaoDich")]
        public virtual GiaoDich GiaoDich { get; set; }
    }
}