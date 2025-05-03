using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QL_ThuChi.Models
{
    public class KhachHang
    {
        [Key]
        public string MAKH { get; set; }

        public string MATAIKHOAN { get; set; }

        [ForeignKey("MATAIKHOAN")]
        public TaiKhoan TaiKhoan { get; set; }

        public string? HOTEN { get; set; }
        public DateTime? NGAYSINH { get; set; }  // <- Có thể null
        public string? SODT { get; set; }
        public int XU { get; set; }
        public string? AVATAR { get; set; }      // <- Có thể null
    }
}
