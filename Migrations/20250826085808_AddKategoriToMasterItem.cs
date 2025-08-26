using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIFO_Infineon.Migrations
{
    /// <inheritdoc />
    public partial class AddKategoriToMasterItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Kategori",
                table: "MasterItems",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kategori",
                table: "MasterItems");
        }
    }
}
