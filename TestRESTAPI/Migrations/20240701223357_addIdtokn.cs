using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestRESTAPI.Migrations
{
    /// <inheritdoc />
    public partial class addIdtokn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "idTokn",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "provider",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "idTokn",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "provider",
                table: "AspNetUsers");
        }
    }
}
