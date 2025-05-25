using System.ComponentModel.DataAnnotations;

public class LoaiTien
{
    [Key]
    public int MaLoai { get; set; }

    [Required]
    [StringLength(100)]
    public string TenLoai { get; set; }

    [Required]
    [StringLength(10)]
    public string MenhGia { get; set; }

    [Required]
    [StringLength(5)]
    public string KyHieu { get; set; }
}
