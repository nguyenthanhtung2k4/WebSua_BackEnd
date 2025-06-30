using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHinhAnhSanPhamTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HinhAnhSanPham",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSanPham = table.Column<int>(type: "int", nullable: false),
                    DuongDan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SanPhamMaSua = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HinhAnhSanPham", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HinhAnhSanPham_SanPhamSua_SanPhamMaSua",
                        column: x => x.SanPhamMaSua,
                        principalTable: "SanPhamSua",
                        principalColumn: "MaSua");
                });

            migrationBuilder.CreateIndex(
                name: "IX_HinhAnhSanPham_SanPhamMaSua",
                table: "HinhAnhSanPham",
                column: "SanPhamMaSua");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HinhAnhSanPham");
        }
    }
}
