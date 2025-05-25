using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_ThuChi.Models
{
    public class Vi
    {
        [Key]
        public int MaVi { get; set; }

        [Required]
        [StringLength(100)]
        public string TenVi { get; set; }

        [StringLength(50)]
        public string? LoaiVi { get; set; }

        [StringLength(100)]
        public string? IconVi { get; set; }
    }
}
