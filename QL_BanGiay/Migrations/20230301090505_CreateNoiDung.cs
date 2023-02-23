using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLBanGiay.Migrations
{
    /// <inheritdoc />
    public partial class CreateNoiDung : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NoiDung",
                columns: table => new
                {
                    MaNoiDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaGiay = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    ThongTin = table.Column<string>(type: "nvarchar(max)", unicode: false, maxLength: 10000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NoiDung__356240DFED68AB91", x => x.MaNoiDung);
                    table.ForeignKey(
                        name: "FK__NoiDung__MaGiay__45671F6C",
                        column: x => x.MaGiay,
                        principalTable: "Giay",
                        principalColumn: "MaGiay");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NoiDung_MaGiay",
                table: "NoiDung",
                column: "MaGiay");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NoiDung");
        }
    }
}
