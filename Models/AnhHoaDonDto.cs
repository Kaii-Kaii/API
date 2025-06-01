using System.ComponentModel.DataAnnotations;

namespace QL_ThuChi.Models
{
    public class AnhHoaDonDto
    {
        [Required(ErrorMessage = "Mã giao dịch không được để trống")]
        public int MaGiaoDich { get; set; }

        [Required(ErrorMessage = "Đường dẫn ảnh không được để trống")]
        [StringLength(255, ErrorMessage = "Đường dẫn ảnh không được vượt quá 255 ký tự")]
        public string DuongDanAnh { get; set; }
    }
}