using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QL_BanGiay.Data;

public partial class QlyBanGiayContext : DbContext
{
    public QlyBanGiayContext()
    {
    }

    public QlyBanGiayContext(DbContextOptions<QlyBanGiayContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AnhGiay> AnhGiays { get; set; }

    public virtual DbSet<DiaChi> DiaChis { get; set; }

    public virtual DbSet<DonDat> DonDats { get; set; }

    public virtual DbSet<DonDatCt> DonDatCts { get; set; }

    public virtual DbSet<DonViNhapHang> DonViNhapHangs { get; set; }

    public virtual DbSet<DongSanPham> DongSanPhams { get; set; }

    public virtual DbSet<Giay> Giays { get; set; }

    public virtual DbSet<Huyen> Huyens { get; set; }

    public virtual DbSet<KhoGiay> KhoGiays { get; set; }

    public virtual DbSet<KhuyenMai> KhuyenMais { get; set; }

    public virtual DbSet<KhuyenMaiCt> KhuyenMaiCts { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<NhanHieu> NhanHieus { get; set; }

    public virtual DbSet<NhapHang> NhapHangs { get; set; }

    public virtual DbSet<NhapHangCt> NhapHangCts { get; set; }

    public virtual DbSet<NoiSanXuat> NoiSanXuats { get; set; }

    public virtual DbSet<Quyen> Quyens { get; set; }

    public virtual DbSet<QuyenCt> QuyenCts { get; set; }

    public virtual DbSet<SizeGiay> SizeGiays { get; set; }

    public virtual DbSet<Tinh> Tinhs { get; set; }

    public virtual DbSet<Xa> Xas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\KHOA;Database=Qly_BanGiay;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AnhGiay>(entity =>
        {
            entity.HasKey(e => e.MaAnh).HasName("PK__AnhGiay__356240DFED68AB69");

            entity.ToTable("AnhGiay");

            entity.Property(e => e.MaGiay)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TenAnh)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Url)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.MaGiayNavigation).WithMany(p => p.AnhGiays)
                .HasForeignKey(d => d.MaGiay)
                .HasConstraintName("FK__AnhGiay__MaGiay__22751F6C");
        });

        modelBuilder.Entity<DiaChi>(entity =>
        {
            entity.HasKey(e => e.MaDiaChi).HasName("PK__DiaChi__EB61213EFBD5B0A6");

            entity.ToTable("DiaChi");

            entity.Property(e => e.MaNguoiDung)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MaXa)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TenDiaChi).HasMaxLength(50);

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.DiaChis)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__DiaChi__MaNguoiD__40058253");

            entity.HasOne(d => d.MaXaNavigation).WithMany(p => p.DiaChis)
                .HasForeignKey(d => d.MaXa)
                .HasConstraintName("FK__DiaChi__MaXa__1332DBDC");
        });

        modelBuilder.Entity<DonDat>(entity =>
        {
            entity.HasKey(e => e.MaDonDat).HasName("PK__DonDat__CD361BAC1A0ED217");

            entity.ToTable("DonDat");

            entity.Property(e => e.MaNguoiDung)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.NgayDat).HasColumnType("datetime");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.DonDats)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__DonDat__MaNguoiD__40F9A68C");
        });

        modelBuilder.Entity<DonDatCt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DonDatCT__3214EC07D6983F61");

            entity.ToTable("DonDatCT");

            entity.Property(e => e.MaGiay)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.MaDonDatNavigation).WithMany(p => p.DonDatCts)
                .HasForeignKey(d => d.MaDonDat)
                .HasConstraintName("FK__DonDatCT__MaDonD__3D2915A8");

            entity.HasOne(d => d.MaGiayNavigation).WithMany(p => p.DonDatCts)
                .HasForeignKey(d => d.MaGiay)
                .HasConstraintName("FK__DonDatCT__MaGiay__3C34F16F");
        });

        modelBuilder.Entity<DonViNhapHang>(entity =>
        {
            entity.HasKey(e => e.MaDonViNhap).HasName("PK__DonViNha__DDB1EA7627BB9A6E");

            entity.ToTable("DonViNhapHang");

            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.SoDienThai).HasMaxLength(20);
            entity.Property(e => e.TenDonViNhap).HasMaxLength(255);
        });

        modelBuilder.Entity<DongSanPham>(entity =>
        {
            entity.HasKey(e => e.MaDongSanPham).HasName("PK__BoSuuTap__C72D040D351BB5A7");

            entity.ToTable("DongSanPham");

            entity.Property(e => e.TenDongSanPham).HasMaxLength(100);

            entity.HasOne(d => d.MaNhanHieuNavigation).WithMany(p => p.DongSanPhams)
                .HasForeignKey(d => d.MaNhanHieu)
                .HasConstraintName("FK__BoSuuTap__MaNhan__17F790F9");
        });

        modelBuilder.Entity<Giay>(entity =>
        {
            entity.HasKey(e => e.MaGiay).HasName("PK__Giay__747065AEA19309D6");

            entity.ToTable("Giay");

            entity.Property(e => e.MaGiay)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ChatLieu).HasMaxLength(50);
            entity.Property(e => e.MauSac).HasMaxLength(50);
            entity.Property(e => e.AnhDD).HasMaxLength(50);
            entity.Property(e => e.NgayCn)
                .HasColumnType("datetime")
                .HasColumnName("NgayCN");
            entity.Property(e => e.TenGiay).HasMaxLength(100);

            entity.HasOne(d => d.MaDongSanPhamNavigation).WithMany(p => p.Giays)
                .HasForeignKey(d => d.MaDongSanPham)
                .HasConstraintName("FK__Giay__MaDongSanP__73852659");

            entity.HasOne(d => d.MaNhaSanXuatNavigation).WithMany(p => p.Giays)
                .HasForeignKey(d => d.MaNhaSanXuat)
                .HasConstraintName("FK__Giay__MaNhaSanXu__1EA48E88");
        });

        modelBuilder.Entity<Huyen>(entity =>
        {
            entity.HasKey(e => e.MaHuyen).HasName("districts_pkey");

            entity.ToTable("Huyen");

            entity.Property(e => e.MaHuyen)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.MaTinh)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TenHuyen).HasMaxLength(255);

            entity.HasOne(d => d.MaTinhNavigation).WithMany(p => p.Huyens)
                .HasForeignKey(d => d.MaTinh)
                .HasConstraintName("districts_province_code_fkey");
        });

        modelBuilder.Entity<KhoGiay>(entity =>
        {
            entity.HasKey(e => new { e.MaGiay, e.MaSize }).HasName("PK_Giay_Size");

            entity.ToTable("KhoGiay");

            entity.Property(e => e.MaGiay)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.MaGiayNavigation).WithMany(p => p.KhoGiays)
                .HasForeignKey(d => d.MaGiay)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KhoGiay__MaGiay__31B762FC");

            entity.HasOne(d => d.MaSizeNavigation).WithMany(p => p.KhoGiays)
                .HasForeignKey(d => d.MaSize)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KhoGiay__MaSize__32AB8735");
        });

        modelBuilder.Entity<KhuyenMai>(entity =>
        {
            entity.HasKey(e => e.MaKhuyenMai).HasName("PK__KhuyenMa__6F56B3BDAC5B2217");

            entity.ToTable("KhuyenMai");

            entity.Property(e => e.MaKhuyenMai)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Lydo).HasMaxLength(100);
            entity.Property(e => e.NgayBd)
                .HasColumnType("datetime")
                .HasColumnName("NgayBD");
            entity.Property(e => e.NgayKt)
                .HasColumnType("datetime")
                .HasColumnName("NgayKT");
        });

        modelBuilder.Entity<KhuyenMaiCt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__KhuyenMa__3214EC07D44119D2");

            entity.ToTable("KhuyenMaiCT");

            entity.Property(e => e.MaGiay)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.MaKhuyenMai)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.MaGiayNavigation).WithMany(p => p.KhuyenMaiCts)
                .HasForeignKey(d => d.MaGiay)
                .HasConstraintName("FK__KhuyenMai__MaGia__367C1819");

            entity.HasOne(d => d.MaKhuyenMaiNavigation).WithMany(p => p.KhuyenMaiCts)
                .HasForeignKey(d => d.MaKhuyenMai)
                .HasConstraintName("FK__KhuyenMai__MaKhu__37703C52");
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__NguoiDun__C539D7628EF9F54B");

            entity.ToTable("NguoiDung");

            entity.Property(e => e.MaNguoiDung)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Avatar).HasMaxLength(50);
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("Create_at");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.HoNguoiDung).HasMaxLength(50);
            entity.Property(e => e.Password)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.Sdt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TenNguoiDung).HasMaxLength(50);
        });

        modelBuilder.Entity<NhanHieu>(entity =>
        {
            entity.HasKey(e => e.MaNhanHieu).HasName("PK__NhanHieu__75BD22482D398986");

            entity.ToTable("NhanHieu");

            entity.Property(e => e.TenNhanHieu).HasMaxLength(30);
        });

        modelBuilder.Entity<NhapHang>(entity =>
        {
            entity.HasKey(e => e.MaNhapHang).HasName("PK__NhapHang__42ECBDEA7DF5E4FB");

            entity.ToTable("NhapHang");

            entity.Property(e => e.NgayNhap).HasColumnType("datetime");

            entity.HasOne(d => d.MaDonViNhapNavigation).WithMany(p => p.NhapHangs)
                .HasForeignKey(d => d.MaDonViNhap)
                .HasConstraintName("FK__NhapHang__MaDonV__2A164134");
        });

        modelBuilder.Entity<NhapHangCt>(entity =>
        {
            entity.HasKey(e => e.MaNhapHangCt).HasName("PK__NhapHang__E5FC75A63EABA4DB");

            entity.ToTable("NhapHangCT");

            entity.Property(e => e.MaNhapHangCt).HasColumnName("MaNhapHangCT");
            entity.Property(e => e.MaGiay)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.MaGiayNavigation).WithMany(p => p.NhapHangCts)
                .HasForeignKey(d => d.MaGiay)
                .HasConstraintName("FK__NhapHangC__MaGia__2CF2ADDF");

            entity.HasOne(d => d.MaNhapHangNavigation).WithMany(p => p.NhapHangCts)
                .HasForeignKey(d => d.MaNhapHang)
                .HasConstraintName("FK__NhapHangC__MaNha__2DE6D218");

            entity.HasOne(d => d.MaSizeNavigation).WithMany(p => p.NhapHangCts)
                .HasForeignKey(d => d.MaSize)
                .HasConstraintName("FK__NhapHangC__MaSiz__2EDAF651");
        });

        modelBuilder.Entity<NoiSanXuat>(entity =>
        {
            entity.HasKey(e => e.MaNhaSanXuat).HasName("PK__NoiSanXu__838C17A10BC26D9A");

            entity.ToTable("NoiSanXuat");

            entity.Property(e => e.TenNhaSanXuat)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Quyen>(entity =>
        {
            entity.HasKey(e => e.MaQuyen).HasName("PK__Quyen__1D4B7ED406D1FF37");

            entity.ToTable("Quyen");

            entity.Property(e => e.TenQuyen).HasMaxLength(30);
        });

        modelBuilder.Entity<QuyenCt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__QuyenCT__3214EC2779942593");

            entity.ToTable("QuyenCT");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaNguoiDung)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.QuyenCts)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__QuyenCT__MaNguoi__41EDCAC5");

            entity.HasOne(d => d.MaQuyenNavigation).WithMany(p => p.QuyenCts)
                .HasForeignKey(d => d.MaQuyen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QuyenCT__MaQuyen__3A4CA8FD");
        });

        modelBuilder.Entity<SizeGiay>(entity =>
        {
            entity.HasKey(e => e.MaSize).HasName("PK__SizeGiay__A787E7EDA8016F2A");

            entity.ToTable("SizeGiay");

            entity.Property(e => e.TenSize).HasMaxLength(8);
        });

        modelBuilder.Entity<Tinh>(entity =>
        {
            entity.HasKey(e => e.MaTinh).HasName("provinces_pkey");

            entity.ToTable("Tinh");

            entity.Property(e => e.MaTinh)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TenTinh).HasMaxLength(255);
        });

        modelBuilder.Entity<Xa>(entity =>
        {
            entity.HasKey(e => e.MaXa).HasName("wards_pkey");

            entity.ToTable("Xa");

            entity.Property(e => e.MaXa)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.MaHuyen)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TenXa).HasMaxLength(255);

            entity.HasOne(d => d.MaHuyenNavigation).WithMany(p => p.Xas)
                .HasForeignKey(d => d.MaHuyen)
                .HasConstraintName("wards_district_code_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
