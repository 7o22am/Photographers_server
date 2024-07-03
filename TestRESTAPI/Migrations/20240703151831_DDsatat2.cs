using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestRESTAPI.Migrations
{
    /// <inheritdoc />
    public partial class DDsatat2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "photographerName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "photographerName",
                table: "Orders");
        }
    }
}
