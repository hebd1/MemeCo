using Microsoft.EntityFrameworkCore.Migrations;

namespace MemeCo.Migrations
{
    public partial class template : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "Templates");

            migrationBuilder.AddColumn<int>(
                name: "TempleteID",
                table: "Posts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TempleteID",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "Templates",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
