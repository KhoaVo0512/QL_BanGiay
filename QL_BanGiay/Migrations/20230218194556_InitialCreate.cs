using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLBanGiay.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DonViNhapHang",
                columns: table => new
                {
                    MaDonViNhap = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDonViNhap = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SoDienThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DonViNha__DDB1EA7627BB9A6E", x => x.MaDonViNhap);
                });

            migrationBuilder.CreateTable(
                name: "KhuyenMai",
                columns: table => new
                {
                    MaKhuyenMai = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    NgayBD = table.Column<DateTime>(type: "datetime", nullable: true),
                    NgayKT = table.Column<DateTime>(type: "datetime", nullable: true),
                    Lydo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__KhuyenMa__6F56B3BDAC5B2217", x => x.MaKhuyenMai);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDung",
                columns: table => new
                {
                    MaNguoiDung = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Password = table.Column<string>(type: "varchar(70)", unicode: false, maxLength: 70, nullable: true),
                    HoNguoiDung = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TenNguoiDung = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Sdt = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    GioiTinh = table.Column<int>(type: "int", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Createat = table.Column<DateTime>(name: "Create_at", type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NguoiDun__C539D7628EF9F54B", x => x.MaNguoiDung);
                });

            migrationBuilder.CreateTable(
                name: "NhanHieu",
                columns: table => new
                {
                    MaNhanHieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhanHieu = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NhanHieu__75BD22482D398986", x => x.MaNhanHieu);
                });

            migrationBuilder.CreateTable(
                name: "NoiSanXuat",
                columns: table => new
                {
                    MaNhaSanXuat = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhaSanXuat = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NoiSanXu__838C17A10BC26D9A", x => x.MaNhaSanXuat);
                });

            migrationBuilder.CreateTable(
                name: "Quyen",
                columns: table => new
                {
                    MaQuyen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenQuyen = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Quyen__1D4B7ED406D1FF37", x => x.MaQuyen);
                });

            migrationBuilder.CreateTable(
                name: "SizeGiay",
                columns: table => new
                {
                    MaSize = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenSize = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SizeGiay__A787E7EDA8016F2A", x => x.MaSize);
                });

            migrationBuilder.CreateTable(
                name: "Tinh",
                columns: table => new
                {
                    MaTinh = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    TenTinh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("provinces_pkey", x => x.MaTinh);
                });

            migrationBuilder.CreateTable(
                name: "NhapHang",
                columns: table => new
                {
                    MaNhapHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDonViNhap = table.Column<int>(type: "int", nullable: false),
                    SoHoaDon = table.Column<int>(type: "int", nullable: false),
                    NgayNhap = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NhapHang__42ECBDEA7DF5E4FB", x => x.MaNhapHang);
                    table.ForeignKey(
                        name: "FK__NhapHang__MaDonV__2A164134",
                        column: x => x.MaDonViNhap,
                        principalTable: "DonViNhapHang",
                        principalColumn: "MaDonViNhap",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonDat",
                columns: table => new
                {
                    MaDonDat = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    NgayDat = table.Column<DateTime>(type: "datetime", nullable: true),
                    DaThanhToan = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DonDat__CD361BAC1A0ED217", x => x.MaDonDat);
                    table.ForeignKey(
                        name: "FK__DonDat__MaNguoiD__40F9A68C",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateTable(
                name: "DongSanPham",
                columns: table => new
                {
                    MaDongSanPham = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNhanHieu = table.Column<int>(type: "int", nullable: false),
                    TenDongSanPham = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BoSuuTap__C72D040D351BB5A7", x => x.MaDongSanPham);
                    table.ForeignKey(
                        name: "FK__BoSuuTap__MaNhan__17F790F9",
                        column: x => x.MaNhanHieu,
                        principalTable: "NhanHieu",
                        principalColumn: "MaNhanHieu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuyenCT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    MaQuyen = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__QuyenCT__3214EC2779942593", x => x.ID);
                    table.ForeignKey(
                        name: "FK__QuyenCT__MaNguoi__41EDCAC5",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung");
                    table.ForeignKey(
                        name: "FK__QuyenCT__MaQuyen__3A4CA8FD",
                        column: x => x.MaQuyen,
                        principalTable: "Quyen",
                        principalColumn: "MaQuyen");
                });

            migrationBuilder.CreateTable(
                name: "Huyen",
                columns: table => new
                {
                    MaHuyen = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    TenHuyen = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MaTinh = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("districts_pkey", x => x.MaHuyen);
                    table.ForeignKey(
                        name: "districts_province_code_fkey",
                        column: x => x.MaTinh,
                        principalTable: "Tinh",
                        principalColumn: "MaTinh");
                });

            migrationBuilder.CreateTable(
                name: "Giay",
                columns: table => new
                {
                    MaGiay = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    MaNhaSanXuat = table.Column<int>(type: "int", nullable: true),
                    MaDongSanPham = table.Column<int>(type: "int", nullable: true),
                    TenGiay = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ChatLieu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MauSac = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AnhDD = table.Column<string>(type: "nvarchar(100)", maxLength: 50, nullable: true),
                    GiaBan = table.Column<int>(type: "int", nullable: true),
                    NgayCN = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Giay__747065AEA19309D6", x => x.MaGiay);
                    table.ForeignKey(
                        name: "FK__Giay__MaDongSanP__73852659",
                        column: x => x.MaDongSanPham,
                        principalTable: "DongSanPham",
                        principalColumn: "MaDongSanPham");
                    table.ForeignKey(
                        name: "FK__Giay__MaNhaSanXu__1EA48E88",
                        column: x => x.MaNhaSanXuat,
                        principalTable: "NoiSanXuat",
                        principalColumn: "MaNhaSanXuat");
                });

            migrationBuilder.CreateTable(
                name: "Xa",
                columns: table => new
                {
                    MaXa = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    TenXa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MaHuyen = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("wards_pkey", x => x.MaXa);
                    table.ForeignKey(
                        name: "wards_district_code_fkey",
                        column: x => x.MaHuyen,
                        principalTable: "Huyen",
                        principalColumn: "MaHuyen");
                });

            migrationBuilder.CreateTable(
                name: "AnhGiay",
                columns: table => new
                {
                    MaAnh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaGiay = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    TenAnh = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Url = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AnhGiay__356240DFED68AB69", x => x.MaAnh);
                    table.ForeignKey(
                        name: "FK__AnhGiay__MaGiay__22751F6C",
                        column: x => x.MaGiay,
                        principalTable: "Giay",
                        principalColumn: "MaGiay");
                });

            migrationBuilder.CreateTable(
                name: "DonDatCT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaGiay = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    MaDonDat = table.Column<int>(type: "int", nullable: true),
                    SoLuong = table.Column<int>(type: "int", nullable: true),
                    DonGia = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DonDatCT__3214EC07D6983F61", x => x.Id);
                    table.ForeignKey(
                        name: "FK__DonDatCT__MaDonD__3D2915A8",
                        column: x => x.MaDonDat,
                        principalTable: "DonDat",
                        principalColumn: "MaDonDat");
                    table.ForeignKey(
                        name: "FK__DonDatCT__MaGiay__3C34F16F",
                        column: x => x.MaGiay,
                        principalTable: "Giay",
                        principalColumn: "MaGiay");
                });

            migrationBuilder.CreateTable(
                name: "KhoGiay",
                columns: table => new
                {
                    MaGiay = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    MaSize = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Giay_Size", x => new { x.MaGiay, x.MaSize });
                    table.ForeignKey(
                        name: "FK__KhoGiay__MaGiay__31B762FC",
                        column: x => x.MaGiay,
                        principalTable: "Giay",
                        principalColumn: "MaGiay");
                    table.ForeignKey(
                        name: "FK__KhoGiay__MaSize__32AB8735",
                        column: x => x.MaSize,
                        principalTable: "SizeGiay",
                        principalColumn: "MaSize");
                });

            migrationBuilder.CreateTable(
                name: "KhuyenMaiCT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaGiay = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    MaKhuyenMai = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    Tile = table.Column<double>(type: "float", nullable: true),
                    GiaHienTai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__KhuyenMa__3214EC07D44119D2", x => x.Id);
                    table.ForeignKey(
                        name: "FK__KhuyenMai__MaGia__367C1819",
                        column: x => x.MaGiay,
                        principalTable: "Giay",
                        principalColumn: "MaGiay");
                    table.ForeignKey(
                        name: "FK__KhuyenMai__MaKhu__37703C52",
                        column: x => x.MaKhuyenMai,
                        principalTable: "KhuyenMai",
                        principalColumn: "MaKhuyenMai");
                });

            migrationBuilder.CreateTable(
                name: "NhapHangCT",
                columns: table => new
                {
                    MaNhapHangCT = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaGiay = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    MaNhapHang = table.Column<int>(type: "int", nullable: true),
                    MaSize = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    GiaMua = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NhapHang__E5FC75A63EABA4DB", x => x.MaNhapHangCT);
                    table.ForeignKey(
                        name: "FK__NhapHangC__MaGia__2CF2ADDF",
                        column: x => x.MaGiay,
                        principalTable: "Giay",
                        principalColumn: "MaGiay",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__NhapHangC__MaNha__2DE6D218",
                        column: x => x.MaNhapHang,
                        principalTable: "NhapHang",
                        principalColumn: "MaNhapHang");
                    table.ForeignKey(
                        name: "FK__NhapHangC__MaSiz__2EDAF651",
                        column: x => x.MaSize,
                        principalTable: "SizeGiay",
                        principalColumn: "MaSize",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiaChi",
                columns: table => new
                {
                    MaDiaChi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    MaXa = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    TenDiaChi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DiaChi__EB61213EFBD5B0A6", x => x.MaDiaChi);
                    table.ForeignKey(
                        name: "FK__DiaChi__MaNguoiD__40058253",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung");
                    table.ForeignKey(
                        name: "FK__DiaChi__MaXa__1332DBDC",
                        column: x => x.MaXa,
                        principalTable: "Xa",
                        principalColumn: "MaXa");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnhGiay_MaGiay",
                table: "AnhGiay",
                column: "MaGiay");

            migrationBuilder.CreateIndex(
                name: "IX_DiaChi_MaNguoiDung",
                table: "DiaChi",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_DiaChi_MaXa",
                table: "DiaChi",
                column: "MaXa");

            migrationBuilder.CreateIndex(
                name: "IX_DonDat_MaNguoiDung",
                table: "DonDat",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_DonDatCT_MaDonDat",
                table: "DonDatCT",
                column: "MaDonDat");

            migrationBuilder.CreateIndex(
                name: "IX_DonDatCT_MaGiay",
                table: "DonDatCT",
                column: "MaGiay");

            migrationBuilder.CreateIndex(
                name: "IX_DongSanPham_MaNhanHieu",
                table: "DongSanPham",
                column: "MaNhanHieu");

            migrationBuilder.CreateIndex(
                name: "IX_Giay_MaDongSanPham",
                table: "Giay",
                column: "MaDongSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_Giay_MaNhaSanXuat",
                table: "Giay",
                column: "MaNhaSanXuat");

            migrationBuilder.CreateIndex(
                name: "IX_Huyen_MaTinh",
                table: "Huyen",
                column: "MaTinh");

            migrationBuilder.CreateIndex(
                name: "IX_KhoGiay_MaSize",
                table: "KhoGiay",
                column: "MaSize");

            migrationBuilder.CreateIndex(
                name: "IX_KhuyenMaiCT_MaGiay",
                table: "KhuyenMaiCT",
                column: "MaGiay");

            migrationBuilder.CreateIndex(
                name: "IX_KhuyenMaiCT_MaKhuyenMai",
                table: "KhuyenMaiCT",
                column: "MaKhuyenMai");

            migrationBuilder.CreateIndex(
                name: "IX_NhapHang_MaDonViNhap",
                table: "NhapHang",
                column: "MaDonViNhap");

            migrationBuilder.CreateIndex(
                name: "IX_NhapHangCT_MaGiay",
                table: "NhapHangCT",
                column: "MaGiay");

            migrationBuilder.CreateIndex(
                name: "IX_NhapHangCT_MaNhapHang",
                table: "NhapHangCT",
                column: "MaNhapHang");

            migrationBuilder.CreateIndex(
                name: "IX_NhapHangCT_MaSize",
                table: "NhapHangCT",
                column: "MaSize");

            migrationBuilder.CreateIndex(
                name: "IX_QuyenCT_MaNguoiDung",
                table: "QuyenCT",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_QuyenCT_MaQuyen",
                table: "QuyenCT",
                column: "MaQuyen");

            migrationBuilder.CreateIndex(
                name: "IX_Xa_MaHuyen",
                table: "Xa",
                column: "MaHuyen");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnhGiay");

            migrationBuilder.DropTable(
                name: "DiaChi");

            migrationBuilder.DropTable(
                name: "DonDatCT");

            migrationBuilder.DropTable(
                name: "KhoGiay");

            migrationBuilder.DropTable(
                name: "KhuyenMaiCT");

            migrationBuilder.DropTable(
                name: "NhapHangCT");

            migrationBuilder.DropTable(
                name: "QuyenCT");

            migrationBuilder.DropTable(
                name: "Xa");

            migrationBuilder.DropTable(
                name: "DonDat");

            migrationBuilder.DropTable(
                name: "KhuyenMai");

            migrationBuilder.DropTable(
                name: "Giay");

            migrationBuilder.DropTable(
                name: "NhapHang");

            migrationBuilder.DropTable(
                name: "SizeGiay");

            migrationBuilder.DropTable(
                name: "Quyen");

            migrationBuilder.DropTable(
                name: "Huyen");

            migrationBuilder.DropTable(
                name: "NguoiDung");

            migrationBuilder.DropTable(
                name: "DongSanPham");

            migrationBuilder.DropTable(
                name: "NoiSanXuat");

            migrationBuilder.DropTable(
                name: "DonViNhapHang");

            migrationBuilder.DropTable(
                name: "Tinh");

            migrationBuilder.DropTable(
                name: "NhanHieu");
        }
    }
}
