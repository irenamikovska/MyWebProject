using Microsoft.EntityFrameworkCore.Migrations;

namespace WalksInNature.Data.Migrations
{
    public partial class WalkWithUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Walks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Walks_UserId",
                table: "Walks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Walks_AspNetUsers_UserId",
                table: "Walks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Walks_AspNetUsers_UserId",
                table: "Walks");

            migrationBuilder.DropIndex(
                name: "IX_Walks_UserId",
                table: "Walks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Walks");
        }
    }
}
