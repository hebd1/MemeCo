using Microsoft.EntityFrameworkCore.Migrations;

namespace MemeCo.Migrations
{
    public partial class postIDtoComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TempleteID",
                table: "Posts");

            migrationBuilder.AddColumn<int>(
                name: "TemplateID",
                table: "Posts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemplateID",
                table: "Posts");

            migrationBuilder.AddColumn<int>(
                name: "TempleteID",
                table: "Posts",
                type: "int",
                nullable: true);
        }
    }
}
