using Microsoft.EntityFrameworkCore.Migrations;

namespace WalksInNature.Data.Migrations
{
    public partial class WalkEventIsPublicColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Walks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Walks");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Events");
        }
    }
}
