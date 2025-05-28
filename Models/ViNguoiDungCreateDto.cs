namespace QL_ThuChi.Models
{
    public class ViNguoiDungCreateDto
    {
        public string MaNguoiDung { get; set; } = string.Empty;
        public int MaVi { get; set; }
        public string TenTaiKhoan { get; set; } = string.Empty;
        public int MaLoaiTien { get; set; }
        public string? DienGiai { get; set; }
        public decimal SoDu { get; set; }
    }
}
