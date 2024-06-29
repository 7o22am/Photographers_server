using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestRESTAPI.Migrations
{
    /// <inheritdoc />
    public partial class addOrders2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "location",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "phoneNumber",
                table: "Orders",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "location",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "phoneNumber",
                table: "Orders");
        }
    }
}
