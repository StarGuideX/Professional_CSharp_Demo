using Microsoft.EntityFrameworkCore.Migrations;

namespace MigrationsLib.Migrations
{
    public partial class Image : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                schema: "mc",
                table: "Menus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                schema: "mc",
                table: "Menus");
        }
    }
}
