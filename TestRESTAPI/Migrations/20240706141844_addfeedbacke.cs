using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestRESTAPI.Migrations
{
    /// <inheritdoc />
    public partial class addfeedbacke : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "feedback",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rate",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "feedback",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "rate",
                table: "Orders");
        }
    }
}
