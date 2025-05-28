using QL_ThuChi.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;


namespace QL_ThuChi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<QuyenTruyCap> QuyenTruyCaps { get; set; }
        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<Vi> Vi { get; set; } 
        public DbSet<LoaiTien> LoaiTiens { get; set; }
        public DbSet<ViNguoiDung> ViNguoiDungs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<QuyenTruyCap>().ToTable("QuyenTruyCap");
            modelBuilder.Entity<TaiKhoan>().ToTable("TaiKhoan");
            modelBuilder.Entity<KhachHang>().ToTable("KhachHang");
            modelBuilder.Entity<Vi>().ToTable("Vi");
            modelBuilder.Entity<LoaiTien>().ToTable("LoaiTien");
            modelBuilder.Entity<ViNguoiDung>().ToTable("ViNguoiDung");

            modelBuilder.Entity<ViNguoiDung>()
                .HasKey(vnd => new { vnd.MaNguoiDung, vnd.MaVi, vnd.TenTaiKhoan });
        }
    }

}
