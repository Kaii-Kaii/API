using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QL_ThuChi.Models
{
    public class TaiKhoanDto
    {
        [Key]
        public string MATAIKHOAN { get; set; }

        public string? KhachHang { get; set; }
        public string? MAQUYEN { get; set; }

        [ForeignKey("MAQUYEN")]
        public QuyenTruyCap? QuyenTruyCap { get; set; }

        public string? TENDANGNHAP { get; set; }
        public string? MATKHAU { get; set; }
        public string? EMAIL { get; set; }
        public int? ISEMAILCONFIRMED { get; set; }
        public string? EMAILCONFIRMATIONTOKEN { get; set; }
        public int? OTP { get; set; }

        
    }
}
