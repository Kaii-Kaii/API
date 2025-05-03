using System.ComponentModel.DataAnnotations;

namespace QL_ThuChi.Models
{
    public class QuyenTruyCap
    {
        [Key]
        public string MAQUYEN { get; set; }

        [Required]
        public string TENQUYEN { get; set; }

        public ICollection<TaiKhoan> TaiKhoans { get; set; }
    }
}
