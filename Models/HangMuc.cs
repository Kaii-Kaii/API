using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_ThuChi.Models
{
    public class HangMuc
    {
        [Key]
        public string MAHANGMUC { get; set; }

        public string MAKH { get; set; }

        [ForeignKey("MAKH")]

        public string TENHANGMUC { get; set; }

        public string? ICON { get; set; } // Đường dẫn hoặc tên icon, có thể null

        public string LOAI { get; set; } // 'chi', 'thu', 'vayno',...

        public bool HAYDUNG { get; set; } // true nếu là mục hay dùng


    }
}