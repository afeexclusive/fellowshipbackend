using Microsoft.EntityFrameworkCore.Migrations;

namespace Fellowship.Migrations
{
    public partial class extendFellow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Fellows",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Fellows",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Fellows",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Fellows");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Fellows");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Fellows");
        }
    }
}
