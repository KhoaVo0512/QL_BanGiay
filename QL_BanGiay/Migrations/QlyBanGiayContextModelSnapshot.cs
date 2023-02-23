﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QL_BanGiay.Data;

#nullable disable

namespace QLBanGiay.Migrations
{
    [DbContext(typeof(QlyBanGiayContext))]
    partial class QlyBanGiayContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("QL_BanGiay.Data.AnhGiay", b =>
                {
                    b.Property<int>("MaAnh")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaAnh"));

                    b.Property<string>("MaGiay")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("TenAnh")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Url")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.HasKey("MaAnh")
                        .HasName("PK__AnhGiay__356240DFED68AB69");

                    b.HasIndex("MaGiay");

                    b.ToTable("AnhGiay", (string)null);
                });

            modelBuilder.Entity("QL_BanGiay.Data.DiaChi", b =>
                {
                    b.Property<int>("MaDiaChi")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaDiaChi"));

                    b.Property<string>("MaNguoiDung")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("MaXa")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("TenDiaChi")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("MaDiaChi")
                        .HasName("PK__DiaChi__EB61213EFBD5B0A6");

                    b.HasIndex("MaNguoiDung");

                    b.HasIndex("MaXa");

                    b.ToTable("DiaChi", (string)null);
                });

            modelBuilder.Entity("QL_BanGiay.Data.DonDat", b =>
                {
                    b.Property<int>("MaDonDat")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaDonDat"));

                    b.Property<bool?>("DaThanhToan")
                        .HasColumnType("bit");

                    b.Property<string>("MaNguoiDung")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)");

                    b.Property<DateTime?>("NgayDat")
                        .HasColumnType("datetime");

                    b.HasKey("MaDonDat")
                        .HasName("PK__DonDat__CD361BAC1A0ED217");

                    b.HasIndex("MaNguoiDung");

                    b.ToTable("DonDat", (string)null);
                });

            modelBuilder.Entity("QL_BanGiay.Data.DonDatCt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("DonGia")
                        .HasColumnType("float");

                    b.Property<int?>("MaDonDat")
                        .HasColumnType("int");

                    b.Property<string>("MaGiay")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<int?>("SoLuong")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__DonDatCT__3214EC07D6983F61");

                    b.HasIndex("MaDonDat");

                    b.HasIndex("MaGiay");

                    b.ToTable("DonDatCT", (string)null);
                });

            modelBuilder.Entity("QL_BanGiay.Data.DonViNhapHang", b =>
                {
                    b.Property<int>("MaDonViNhap")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaDonViNhap"));

                    b.Property<string>("DiaChi")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("SoDienThai")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("TenDonViNhap")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("MaDonViNhap")
                        .HasName("PK__DonViNha__DDB1EA7627BB9A6E");

                    b.ToTable("DonViNhapHang", (string)null);
                });

            modelBuilder.Entity("QL_BanGiay.Data.DongSanPham", b =>
                {
                    b.Property<int>("MaDongSanPham")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaDongSanPham"));

                    b.Property<int?>("MaNhanHieu")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("TenDongSanPham")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("MaDongSanPham")
                        .HasName("PK__BoSuuTap__C72D040D351BB5A7");

                    b.HasIndex("MaNhanHieu");

                    b.ToTable("DongSanPham", (string)null);
                });

            modelBuilder.Entity("QL_BanGiay.Data.Giay", b =>
                {
                    b.Property<string>("MaGiay")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("AnhDD")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ChatLieu")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("GiaBan")
                        .HasColumnType("int");

                    b.Property<int?>("MaDongSanPham")
                        .HasColumnType("int");

                    b.Property<int?>("MaNhaSanXuat")
                        .HasColumnType("int");

                    b.Property<string>("MauSac")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("NgayCn")
                        .HasColumnType("datetime")
                        .HasColumnName("NgayCN");

                    b.Property<string>("TenGiay")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool?>("TrangThai")
                        .HasColumnType("bit");

                    b.HasKey("MaGiay")
                        .HasName("PK__Giay__747065AEA19309D6");

                    b.HasIndex("MaDongSanPham");

                    b.HasIndex("MaNhaSanXuat");

                    b.ToTable("Giay", (string)null);
                });

            modelBuilder.Entity("QL_BanGiay.Data.Huyen", b =>
                {
                    b.Property<string>("MaHuyen")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("MaTinh")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("TenHuyen")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("MaHuyen")
                        .HasName("districts_pkey");

                    b.HasIndex("MaTinh");

                    b.ToTable("Huyen", (string)null);
                });

            modelBuilder.Entity("QL_BanGiay.Data.KhoGiay", b =>
                {
                    b.Property<string>("MaGiay")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<int>("MaSize")
                        .HasColumnType("int");

                    b.Property<int?>("SoLuong")
                        .HasColumnType("int");

                    b.HasKey("MaGiay", "MaSize")
                        .HasName("PK_Giay_Size");

                    b.HasIndex("MaSize");

                    b.ToTable("KhoGiay", (string)null);
                });

            modelBuilder.Entity("QL_BanGiay.Data.KhuyenMai", b =>
                {
                    b.Property<string>("MaKhuyenMai")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Lydo")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("NgayBd")
                        .HasColumnType("datetime")
                        .HasColumnName("NgayBD");

                    b.Property<DateTime?>("NgayKt")
                        .HasColumnType("datetime")
                        .HasColumnName("NgayKT");

                    b.HasKey("MaKhuyenMai")
                        .HasName("PK__KhuyenMa__6F56B3BDAC5B2217");

                    b.ToTable("KhuyenMai", (string)null);
                });

            modelBuilder.Entity("QL_BanGiay.Data.KhuyenMaiCt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("GiaHienTai")
                        .HasColumnType("int");

                    b.Property<string>("MaGiay")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("MaKhuyenMai")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)");

                    b.Property<double?>("Tile")
                        .HasColumnType("float");

                    b.HasKey("Id")
                        .HasName("PK__KhuyenMa__3214EC07D44119D2");

                    b.HasIndex("MaGiay");

                    b.HasIndex("MaKhuyenMai");

                    b.ToTable("KhuyenMaiCT", (string)null);
                });

            modelBuilder.Entity("QL_BanGiay.Data.NguoiDung", b =>
                {
                    b.Property<string>("MaNguoiDung")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Avatar")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("datetime")
                        .HasColumnName("Create_at");

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("GioiTinh")
                        .HasColumnType("int");

                    b.Property<string>("HoNguoiDung")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .HasMaxLength(70)
                        .IsUnicode(false)
                        .HasColumnType("varchar(70)");

                    b.Property<string>("Sdt")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("char(10)")
                        .IsFixedLength();

                    b.Property<string>("TenNguoiDung")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("MaNguoiDung")
                        .HasName("PK__NguoiDun__C539D7628EF9F54B");

                    b.ToTable("NguoiDung", (string)null);
                });

            modelBuilder.Entity("QL_BanGiay.Data.NhanHieu", b =>
                {
                    b.Property<int>("MaNhanHieu")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaNhanHieu"));

                    b.Property<string>("TenNhanHieu")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("MaNhanHieu")
                        .HasName("PK__NhanHieu__75BD22482D398986");

                    b.ToTable("NhanHieu", (string)null);
                });

            modelBuilder.Entity("QL_BanGiay.Data.NhapHang", b =>
                {
                    b.Property<int>("MaNhapHang")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaNhapHang"));

                    b.Property<int?>("MaDonViNhap")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<DateTime?>("NgayNhap")
                        .HasColumnType("datetime");

                    b.Property<int?>("SoHoaDon")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("MaNhapHang")
                        .HasName("PK__NhapHang__42ECBDEA7DF5E4FB");

                    b.HasIndex("MaDonViNhap");

                    b.ToTable("NhapHang", (string)null);
                });

            modelBuilder.Entity("QL_BanGiay.Data.NhapHangCt", b =>
                {
                    b.Property<int>("MaNhapHangCt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("MaNhapHangCT");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaNhapHangCt"));

                    b.Property<int?>("GiaMua")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("MaGiay")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<int?>("MaNhapHang")
                        .HasColumnType("int");

                    b.Property<int?>("MaSize")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("SoLuong")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("MaNhapHangCt")
                        .HasName("PK__NhapHang__E5FC75A63EABA4DB");

                    b.HasIndex("MaGiay");

                    b.HasIndex("MaNhapHang");

                    b.HasIndex("MaSize");

                    b.ToTable("NhapHangCT", (string)null);
                });

            modelBuilder.Entity("QL_BanGiay.Data.NoiDung", b =>
                {
                    b.Property<int>("MaNoiDung")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaNoiDung"));

                    b.Property<string>("MaGiay")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("ThongTin")
                        .HasMaxLength(10000)
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.HasKey("MaNoiDung")
                        .HasName("PK__NoiDung__356240DFED68AB91");

                    b.HasIndex("MaGiay");

                    b.ToTable("NoiDung", (string)null);
                });

            modelBuilder.Entity("QL_BanGiay.Data.NoiSanXuat", b =>
                {
                    b.Property<int>("MaNhaSanXuat")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaNhaSanXuat"));

                    b.Property<string>("TenNhaSanXuat")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.HasKey("MaNhaSanXuat")
                        .HasName("PK__NoiSanXu__838C17A10BC26D9A");

                    b.ToTable("NoiSanXuat", (string)null);
                });

            modelBuilder.Entity("QL_BanGiay.Data.Quyen", b =>
                {
                    b.Property<int>("MaQuyen")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaQuyen"));

                    b.Property<string>("TenQuyen")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("MaQuyen")
                        .HasName("PK__Quyen__1D4B7ED406D1FF37");

                    b.ToTable("Quyen", (string)null);
                });

            modelBuilder.Entity("QL_BanGiay.Data.QuyenCt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("MaNguoiDung")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)");

                    b.Property<int>("MaQuyen")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__QuyenCT__3214EC2779942593");

                    b.HasIndex("MaNguoiDung");

                    b.HasIndex("MaQuyen");

                    b.ToTable("QuyenCT", (string)null);
                });

            modelBuilder.Entity("QL_BanGiay.Data.SizeGiay", b =>
                {
                    b.Property<int>("MaSize")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaSize"));

                    b.Property<string>("TenSize")
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.HasKey("MaSize")
                        .HasName("PK__SizeGiay__A787E7EDA8016F2A");

                    b.ToTable("SizeGiay", (string)null);
                });

            modelBuilder.Entity("QL_BanGiay.Data.Tinh", b =>
                {
                    b.Property<string>("MaTinh")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("TenTinh")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("MaTinh")
                        .HasName("provinces_pkey");

                    b.ToTable("Tinh", (string)null);
                });

            modelBuilder.Entity("QL_BanGiay.Data.Xa", b =>
                {
                    b.Property<string>("MaXa")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("MaHuyen")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("TenXa")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("MaXa")
                        .HasName("wards_pkey");

                    b.HasIndex("MaHuyen");

                    b.ToTable("Xa", (string)null);
                });

            modelBuilder.Entity("QL_BanGiay.Data.AnhGiay", b =>
                {
                    b.HasOne("QL_BanGiay.Data.Giay", "MaGiayNavigation")
                        .WithMany("AnhGiays")
                        .HasForeignKey("MaGiay")
                        .HasConstraintName("FK__AnhGiay__MaGiay__22751F6C");

                    b.Navigation("MaGiayNavigation");
                });

            modelBuilder.Entity("QL_BanGiay.Data.DiaChi", b =>
                {
                    b.HasOne("QL_BanGiay.Data.NguoiDung", "MaNguoiDungNavigation")
                        .WithMany("DiaChis")
                        .HasForeignKey("MaNguoiDung")
                        .HasConstraintName("FK__DiaChi__MaNguoiD__40058253");

                    b.HasOne("QL_BanGiay.Data.Xa", "MaXaNavigation")
                        .WithMany("DiaChis")
                        .HasForeignKey("MaXa")
                        .HasConstraintName("FK__DiaChi__MaXa__1332DBDC");

                    b.Navigation("MaNguoiDungNavigation");

                    b.Navigation("MaXaNavigation");
                });

            modelBuilder.Entity("QL_BanGiay.Data.DonDat", b =>
                {
                    b.HasOne("QL_BanGiay.Data.NguoiDung", "MaNguoiDungNavigation")
                        .WithMany("DonDats")
                        .HasForeignKey("MaNguoiDung")
                        .HasConstraintName("FK__DonDat__MaNguoiD__40F9A68C");

                    b.Navigation("MaNguoiDungNavigation");
                });

            modelBuilder.Entity("QL_BanGiay.Data.DonDatCt", b =>
                {
                    b.HasOne("QL_BanGiay.Data.DonDat", "MaDonDatNavigation")
                        .WithMany("DonDatCts")
                        .HasForeignKey("MaDonDat")
                        .HasConstraintName("FK__DonDatCT__MaDonD__3D2915A8");

                    b.HasOne("QL_BanGiay.Data.Giay", "MaGiayNavigation")
                        .WithMany("DonDatCts")
                        .HasForeignKey("MaGiay")
                        .HasConstraintName("FK__DonDatCT__MaGiay__3C34F16F");

                    b.Navigation("MaDonDatNavigation");

                    b.Navigation("MaGiayNavigation");
                });

            modelBuilder.Entity("QL_BanGiay.Data.DongSanPham", b =>
                {
                    b.HasOne("QL_BanGiay.Data.NhanHieu", "MaNhanHieuNavigation")
                        .WithMany("DongSanPhams")
                        .HasForeignKey("MaNhanHieu")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__BoSuuTap__MaNhan__17F790F9");

                    b.Navigation("MaNhanHieuNavigation");
                });

            modelBuilder.Entity("QL_BanGiay.Data.Giay", b =>
                {
                    b.HasOne("QL_BanGiay.Data.DongSanPham", "MaDongSanPhamNavigation")
                        .WithMany("Giays")
                        .HasForeignKey("MaDongSanPham")
                        .HasConstraintName("FK__Giay__MaDongSanP__73852659");

                    b.HasOne("QL_BanGiay.Data.NoiSanXuat", "MaNhaSanXuatNavigation")
                        .WithMany("Giays")
                        .HasForeignKey("MaNhaSanXuat")
                        .HasConstraintName("FK__Giay__MaNhaSanXu__1EA48E88");

                    b.Navigation("MaDongSanPhamNavigation");

                    b.Navigation("MaNhaSanXuatNavigation");
                });

            modelBuilder.Entity("QL_BanGiay.Data.Huyen", b =>
                {
                    b.HasOne("QL_BanGiay.Data.Tinh", "MaTinhNavigation")
                        .WithMany("Huyens")
                        .HasForeignKey("MaTinh")
                        .HasConstraintName("districts_province_code_fkey");

                    b.Navigation("MaTinhNavigation");
                });

            modelBuilder.Entity("QL_BanGiay.Data.KhoGiay", b =>
                {
                    b.HasOne("QL_BanGiay.Data.Giay", "MaGiayNavigation")
                        .WithMany("KhoGiays")
                        .HasForeignKey("MaGiay")
                        .IsRequired()
                        .HasConstraintName("FK__KhoGiay__MaGiay__31B762FC");

                    b.HasOne("QL_BanGiay.Data.SizeGiay", "MaSizeNavigation")
                        .WithMany("KhoGiays")
                        .HasForeignKey("MaSize")
                        .IsRequired()
                        .HasConstraintName("FK__KhoGiay__MaSize__32AB8735");

                    b.Navigation("MaGiayNavigation");

                    b.Navigation("MaSizeNavigation");
                });

            modelBuilder.Entity("QL_BanGiay.Data.KhuyenMaiCt", b =>
                {
                    b.HasOne("QL_BanGiay.Data.Giay", "MaGiayNavigation")
                        .WithMany("KhuyenMaiCts")
                        .HasForeignKey("MaGiay")
                        .HasConstraintName("FK__KhuyenMai__MaGia__367C1819");

                    b.HasOne("QL_BanGiay.Data.KhuyenMai", "MaKhuyenMaiNavigation")
                        .WithMany("KhuyenMaiCts")
                        .HasForeignKey("MaKhuyenMai")
                        .HasConstraintName("FK__KhuyenMai__MaKhu__37703C52");

                    b.Navigation("MaGiayNavigation");

                    b.Navigation("MaKhuyenMaiNavigation");
                });

            modelBuilder.Entity("QL_BanGiay.Data.NhapHang", b =>
                {
                    b.HasOne("QL_BanGiay.Data.DonViNhapHang", "MaDonViNhapNavigation")
                        .WithMany("NhapHangs")
                        .HasForeignKey("MaDonViNhap")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__NhapHang__MaDonV__2A164134");

                    b.Navigation("MaDonViNhapNavigation");
                });

            modelBuilder.Entity("QL_BanGiay.Data.NhapHangCt", b =>
                {
                    b.HasOne("QL_BanGiay.Data.Giay", "MaGiayNavigation")
                        .WithMany("NhapHangCts")
                        .HasForeignKey("MaGiay")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__NhapHangC__MaGia__2CF2ADDF");

                    b.HasOne("QL_BanGiay.Data.NhapHang", "MaNhapHangNavigation")
                        .WithMany("NhapHangCts")
                        .HasForeignKey("MaNhapHang")
                        .HasConstraintName("FK__NhapHangC__MaNha__2DE6D218");

                    b.HasOne("QL_BanGiay.Data.SizeGiay", "MaSizeNavigation")
                        .WithMany("NhapHangCts")
                        .HasForeignKey("MaSize")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__NhapHangC__MaSiz__2EDAF651");

                    b.Navigation("MaGiayNavigation");

                    b.Navigation("MaNhapHangNavigation");

                    b.Navigation("MaSizeNavigation");
                });

            modelBuilder.Entity("QL_BanGiay.Data.NoiDung", b =>
                {
                    b.HasOne("QL_BanGiay.Data.Giay", "MaGiayNavigation")
                        .WithMany("NoiDungs")
                        .HasForeignKey("MaGiay")
                        .HasConstraintName("FK__NoiDung__MaGiay__45671F6C");

                    b.Navigation("MaGiayNavigation");
                });

            modelBuilder.Entity("QL_BanGiay.Data.QuyenCt", b =>
                {
                    b.HasOne("QL_BanGiay.Data.NguoiDung", "MaNguoiDungNavigation")
                        .WithMany("QuyenCts")
                        .HasForeignKey("MaNguoiDung")
                        .HasConstraintName("FK__QuyenCT__MaNguoi__41EDCAC5");

                    b.HasOne("QL_BanGiay.Data.Quyen", "MaQuyenNavigation")
                        .WithMany("QuyenCts")
                        .HasForeignKey("MaQuyen")
                        .IsRequired()
                        .HasConstraintName("FK__QuyenCT__MaQuyen__3A4CA8FD");

                    b.Navigation("MaNguoiDungNavigation");

                    b.Navigation("MaQuyenNavigation");
                });

            modelBuilder.Entity("QL_BanGiay.Data.Xa", b =>
                {
                    b.HasOne("QL_BanGiay.Data.Huyen", "MaHuyenNavigation")
                        .WithMany("Xas")
                        .HasForeignKey("MaHuyen")
                        .HasConstraintName("wards_district_code_fkey");

                    b.Navigation("MaHuyenNavigation");
                });

            modelBuilder.Entity("QL_BanGiay.Data.DonDat", b =>
                {
                    b.Navigation("DonDatCts");
                });

            modelBuilder.Entity("QL_BanGiay.Data.DonViNhapHang", b =>
                {
                    b.Navigation("NhapHangs");
                });

            modelBuilder.Entity("QL_BanGiay.Data.DongSanPham", b =>
                {
                    b.Navigation("Giays");
                });

            modelBuilder.Entity("QL_BanGiay.Data.Giay", b =>
                {
                    b.Navigation("AnhGiays");

                    b.Navigation("DonDatCts");

                    b.Navigation("KhoGiays");

                    b.Navigation("KhuyenMaiCts");

                    b.Navigation("NhapHangCts");

                    b.Navigation("NoiDungs");
                });

            modelBuilder.Entity("QL_BanGiay.Data.Huyen", b =>
                {
                    b.Navigation("Xas");
                });

            modelBuilder.Entity("QL_BanGiay.Data.KhuyenMai", b =>
                {
                    b.Navigation("KhuyenMaiCts");
                });

            modelBuilder.Entity("QL_BanGiay.Data.NguoiDung", b =>
                {
                    b.Navigation("DiaChis");

                    b.Navigation("DonDats");

                    b.Navigation("QuyenCts");
                });

            modelBuilder.Entity("QL_BanGiay.Data.NhanHieu", b =>
                {
                    b.Navigation("DongSanPhams");
                });

            modelBuilder.Entity("QL_BanGiay.Data.NhapHang", b =>
                {
                    b.Navigation("NhapHangCts");
                });

            modelBuilder.Entity("QL_BanGiay.Data.NoiSanXuat", b =>
                {
                    b.Navigation("Giays");
                });

            modelBuilder.Entity("QL_BanGiay.Data.Quyen", b =>
                {
                    b.Navigation("QuyenCts");
                });

            modelBuilder.Entity("QL_BanGiay.Data.SizeGiay", b =>
                {
                    b.Navigation("KhoGiays");

                    b.Navigation("NhapHangCts");
                });

            modelBuilder.Entity("QL_BanGiay.Data.Tinh", b =>
                {
                    b.Navigation("Huyens");
                });

            modelBuilder.Entity("QL_BanGiay.Data.Xa", b =>
                {
                    b.Navigation("DiaChis");
                });
#pragma warning restore 612, 618
        }
    }
}
