using QL_ThuChi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ViNguoiDung
{
    [Key, Column(Order = 0)]
    [StringLength(20)]
    public string MaNguoiDung { get; set; } = string.Empty;

    [Key, Column(Order = 1)]
    public int MaVi { get; set; }

    [Key, Column(Order = 2)]
    [StringLength(100)]
    public string TenTaiKhoan { get; set; } = string.Empty;

    public int MaLoaiTien { get; set; }

    [StringLength(255)]
    public string? DienGiai { get; set; }

    public decimal SoDu { get; set; }

    // Navigation properties
    [ForeignKey(nameof(MaVi))]
    public Vi Vi { get; set; } = null!;

    [ForeignKey(nameof(MaLoaiTien))]
    public LoaiTien LoaiTien { get; set; } = null!;

    [ForeignKey(nameof(MaNguoiDung))]
    public KhachHang KhachHang { get; set; } = null!;
}
