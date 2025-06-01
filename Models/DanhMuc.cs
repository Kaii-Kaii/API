using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_ThuChi.Models
{
    public class DanhMuc
    {
        [Key]
        public int MaDanhMuc { get; set; }

        [Required]
        [StringLength(100)]
        public string TenDanhMuc { get; set; }

        [Required]
        public int MaLoaiDanhMuc { get; set; }

        // Navigation properties
        [ForeignKey("MaLoaiDanhMuc")]
        public Loai Loai { get; set; }
    }
}