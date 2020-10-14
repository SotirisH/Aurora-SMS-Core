using Microsoft.EntityFrameworkCore.Migrations;

namespace Aurora.Insurance.Data.Migrations
{
    public partial class AttachementsAddType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                "Type",
                "Attachment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "Type",
                "Attachment");
        }
    }
}
