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
        public DbSet<GiaoDich> GiaoDichs { get; set; }
        public DbSet<AnhHoaDon> AnhHoaDons { get; set; }
        public DbSet<LichSuGiaoDich> LichSuGiaoDichs { get; set; }

        public DbSet<HangMuc> HangMucs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<QuyenTruyCap>().ToTable("QuyenTruyCap");
            modelBuilder.Entity<TaiKhoan>().ToTable("TaiKhoan");
            modelBuilder.Entity<KhachHang>().ToTable("KhachHang");
            modelBuilder.Entity<Vi>().ToTable("Vi");
            modelBuilder.Entity<LoaiTien>().ToTable("LoaiTien");
            modelBuilder.Entity<ViNguoiDung>().ToTable("ViNguoiDung");
            modelBuilder.Entity<GiaoDich>().ToTable("GiaoDich");
            modelBuilder.Entity<AnhHoaDon>().ToTable("AnhHoaDon");
            modelBuilder.Entity<LichSuGiaoDich>().ToTable("LichSuGiaoDich");
            modelBuilder.Entity<HangMuc>().ToTable("HangMuc");

            modelBuilder.Entity<ViNguoiDung>()
                .HasKey(vnd => new { vnd.MaNguoiDung, vnd.MaVi, vnd.TenTaiKhoan });

            //modelBuilder.Entity<DanhMucNguoiDung>()
            //    .HasKey(dm => new { dm.MaDanhMucNguoiDung, dm.MaNguoiDung });

            modelBuilder.Entity<GiaoDich>()
                .HasOne(g => g.KhachHang)
                .WithMany()
                .HasForeignKey(g => g.MaNguoiDung)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GiaoDich>()
                .HasOne(g => g.Vi)
                .WithMany()
                .HasForeignKey(g => g.MaVi)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GiaoDich>()
                .HasOne(g => g.ViNhan)
                .WithMany()
                .HasForeignKey(g => g.MaViNhan)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<DanhMucNguoiDung>()
            //    .HasOne(dm => dm.KhachHang)
            //    .WithMany()
            //    .HasForeignKey(dm => dm.MaNguoiDung)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<DanhMuc>()
            //    .HasOne(dm => dm.Loai)
            //    .WithMany()
            //    .HasForeignKey(dm => dm.MaLoaiDanhMuc)
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HangMuc>()
    .HasKey(h => new { h.MAHANGMUC, h.MaNguoiDung });



            modelBuilder.Entity<AnhHoaDon>()
                .HasOne(a => a.GiaoDich)
                .WithMany()
                .HasForeignKey(a => a.MaGiaoDich)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LichSuGiaoDich>()
                .HasOne(l => l.GiaoDich)
                .WithMany()
                .HasForeignKey(l => l.MaGiaoDich)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LichSuGiaoDich>()
                .HasOne(l => l.KhachHang)
                .WithMany()
                .HasForeignKey(l => l.ThucHienBoi)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<GiaoDich>()
                .HasOne(g => g.HangMuc)
                .WithMany()
                .HasForeignKey(g => new { g.MaHangMuc, g.MaNguoiDung })
                .HasPrincipalKey(h => new { h.MAHANGMUC, h.MaNguoiDung })
                .OnDelete(DeleteBehavior.Restrict);

        }
    }

}
