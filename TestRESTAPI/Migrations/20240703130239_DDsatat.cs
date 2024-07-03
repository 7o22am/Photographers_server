using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestRESTAPI.Migrations
{
    /// <inheritdoc />
    public partial class DDsatat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "stata",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "stata",
                table: "Orders");
        }
    }
}
