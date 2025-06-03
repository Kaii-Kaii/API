using System.ComponentModel.DataAnnotations;

namespace QL_ThuChi.Models
{
    public class HangMuc
    {
        [Key]
        public string MAHANGMUC { get; set; }

        public string MaNguoiDung { get; set; } // Đúng tên cột trong DB

        public string TENHANGMUC { get; set; }
        public string? ICON { get; set; }
        public string LOAI { get; set; }
        public bool HAYDUNG { get; set; }
        public decimal? sotienhientai { get; set; }
        public decimal? toida { get; set; }
    }
}