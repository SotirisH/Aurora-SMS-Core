using Microsoft.EntityFrameworkCore.Migrations;

namespace Aurora.Insurance.Data.Migrations
{
    public partial class Addcompanylogo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Company",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LogoData",
                table: "Company",
                unicode: false,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "LogoData",
                table: "Company");
        }
    }
}
