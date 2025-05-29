using System.ComponentModel.DataAnnotations;

namespace QL_ThuChi.Models
{
    public class Loai
    {
        [Key]
        public int MaLoai { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression("^(Thu|Chi)$", ErrorMessage = "TenLoai must be either 'Thu' or 'Chi'")]
        public string TenLoai { get; set; }
    }
}