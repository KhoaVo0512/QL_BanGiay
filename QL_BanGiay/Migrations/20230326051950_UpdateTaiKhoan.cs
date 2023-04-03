using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLBanGiay.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTaiKhoan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiaChi_NguoiDung_MaNguoiDungNavigationMaNguoiDung",
                table: "DiaChi");
            migrationBuilder.DropIndex(
               name: "IX_QuyenCT_NguoiDungMaNguoiDung",
               table: "QuyenCT");
            migrationBuilder.DropIndex(
                name: "IX_DiaChi_MaNguoiDungNavigationMaNguoiDung",
                table: "DiaChi");

            migrationBuilder.DropColumn(
                name: "MaNguoiDungNavigationMaNguoiDung",
                table: "DiaChi");

            migrationBuilder.AddForeignKey(
                name: "FK__DiaChi__NguoiDung__1740DBDC",
                table: "DiaChi",
                column: "MaXa",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");
            migrationBuilder.DropColumn(
                name: "NguoiDungMaNguoiDung",
                table: "QuyenCT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__DiaChi__NguoiDung__1740DBDC",
                table: "DiaChi");

            migrationBuilder.AddColumn<string>(
                name: "MaNguoiDungNavigationMaNguoiDung",
                table: "DiaChi",
                type: "varchar(10)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiaChi_MaNguoiDungNavigationMaNguoiDung",
                table: "DiaChi",
                column: "MaNguoiDungNavigationMaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_DiaChi_NguoiDung_MaNguoiDungNavigationMaNguoiDung",
                table: "DiaChi",
                column: "MaNguoiDungNavigationMaNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");
        }
    }
}
