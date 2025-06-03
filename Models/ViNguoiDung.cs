using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_ThuChi.Models
{
    public class ViNguoiDung
    {
        [Key, Column(Order = 0)]
        public string MaNguoiDung { get; set; }

        [Key, Column(Order = 1)]
        public int MaVi { get; set; }

        [Key, Column(Order = 2)]
        public string TenTaiKhoan { get; set; }

        public int MaLoaiTien { get; set; }

        public string? DienGiai { get; set; }

        public decimal SoDu { get; set; }
        public decimal? SoDuKhac { get; set; }

        // Navigation properties
        [ForeignKey("MaVi")]
        public Vi Vi { get; set; }

        [ForeignKey("MaLoaiTien")]
        public LoaiTien LoaiTien { get; set; }

        [ForeignKey("MaNguoiDung")]
        public KhachHang KhachHang { get; set; }
    }
}
