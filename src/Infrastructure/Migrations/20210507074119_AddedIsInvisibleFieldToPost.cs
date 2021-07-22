using Microsoft.EntityFrameworkCore.Migrations;

namespace Blogpost.Infrastructure.Migrations
{
    public partial class AddedIsInvisibleFieldToPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "Posts");
        }
    }
}
