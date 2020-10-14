using Microsoft.EntityFrameworkCore.Migrations;

namespace Aurora.Insurance.Data.Migrations
{
    public partial class Addcompanylogo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                "IsActive",
                "Company",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                "LogoData",
                "Company",
                unicode: false,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "IsActive",
                "Company");

            migrationBuilder.DropColumn(
                "LogoData",
                "Company");
        }
    }
}
