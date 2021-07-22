using Microsoft.EntityFrameworkCore.Migrations;

namespace Blogpost.Infrastructure.Migrations
{
    public partial class RefreshTokensCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                table: "Posts",
                newName: "LastModifiedById");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                table: "Comments",
                newName: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_LastModifiedById",
                table: "Posts",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_LastModifiedById",
                table: "Comments",
                column: "LastModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Profiles_LastModifiedById",
                table: "Comments",
                column: "LastModifiedById",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Profiles_LastModifiedById",
                table: "Posts",
                column: "LastModifiedById",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Profiles_LastModifiedById",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Profiles_LastModifiedById",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_Posts_LastModifiedById",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Comments_LastModifiedById",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "LastModifiedById",
                table: "Posts",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "LastModifiedById",
                table: "Comments",
                newName: "LastModifiedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
