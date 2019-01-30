using Microsoft.EntityFrameworkCore.Migrations;

namespace TheSongList.Migrations
{
    public partial class episodeDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AirTime",
                table: "Seasons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AirDate",
                table: "Episodes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Director",
                table: "Episodes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProdCode",
                table: "Episodes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Writer",
                table: "Episodes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AirTime",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "AirDate",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "Director",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "ProdCode",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "Writer",
                table: "Episodes");
        }
    }
}
