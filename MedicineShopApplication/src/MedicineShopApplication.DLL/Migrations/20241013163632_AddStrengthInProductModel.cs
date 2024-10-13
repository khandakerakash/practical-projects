using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicineShopApplication.DLL.Migrations
{
    /// <inheritdoc />
    public partial class AddStrengthInProductModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Power_Amount",
                table: "Products",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Power_Unit",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Power_Amount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Power_Unit",
                table: "Products");
        }
    }
}
