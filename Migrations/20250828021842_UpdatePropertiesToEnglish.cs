using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIFO_Infineon.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePropertiesToEnglish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TanggalMasuk",
                table: "StockItems",
                newName: "EntryDate");

            migrationBuilder.RenameColumn(
                name: "Jumlah",
                table: "StockItems",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "NamaItem",
                table: "MasterItems",
                newName: "ItemName");

            migrationBuilder.RenameColumn(
                name: "Kategori",
                table: "MasterItems",
                newName: "ItemDescription");

            migrationBuilder.RenameColumn(
                name: "DeskripsiItem",
                table: "MasterItems",
                newName: "Category");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "StockItems",
                newName: "Jumlah");

            migrationBuilder.RenameColumn(
                name: "EntryDate",
                table: "StockItems",
                newName: "TanggalMasuk");

            migrationBuilder.RenameColumn(
                name: "ItemName",
                table: "MasterItems",
                newName: "NamaItem");

            migrationBuilder.RenameColumn(
                name: "ItemDescription",
                table: "MasterItems",
                newName: "Kategori");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "MasterItems",
                newName: "DeskripsiItem");
        }
    }
}
