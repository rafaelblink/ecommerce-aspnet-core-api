using Microsoft.EntityFrameworkCore.Migrations;

namespace HPlusSport.Migrations
{
    public partial class DecimalPlaces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(19, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19, 4)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(19, 4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19, 2)");
        }
    }
}
