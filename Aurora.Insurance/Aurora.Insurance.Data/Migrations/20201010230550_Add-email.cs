using Microsoft.EntityFrameworkCore.Migrations;

namespace Aurora.Insurance.Data.Migrations
{
    public partial class Addemail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                "EmailAddress",
                "Person",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "EmailAddress",
                "Person");
        }
    }
}
