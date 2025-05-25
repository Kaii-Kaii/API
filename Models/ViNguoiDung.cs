using QL_ThuChi.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class ViNguoiDung
{
    [StringLength(20)]
    public string MaNguoiDung { get; set; } = string.Empty;

    public int MaVi { get; set; }

    [StringLength(100)]
    public string TenTaiKhoan { get; set; } = string.Empty;

    public int MaLoaiTien { get; set; }

    [StringLength(255)]
    public string? DienGiai { get; set; }

    // Khóa ngoại
    [ForeignKey("MaVi")]
    public Vi Vi { get; set; }

    [ForeignKey("MaLoaiTien")]
    public LoaiTien LoaiTien { get; set; }

    [ForeignKey("MaNguoiDung")]
    public KhachHang KhachHang { get; set; }
}
