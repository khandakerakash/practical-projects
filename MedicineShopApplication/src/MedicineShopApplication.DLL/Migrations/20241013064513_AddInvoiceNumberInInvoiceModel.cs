using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicineShopApplication.DLL.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoiceNumberInInvoiceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvoiceNumber",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "Invoices");
        }
    }
}
