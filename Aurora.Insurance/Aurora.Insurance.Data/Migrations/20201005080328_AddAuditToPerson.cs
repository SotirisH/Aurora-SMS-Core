using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aurora.Insurance.Data.Migrations
{
    public partial class AddAuditToPerson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                "CreatedBy",
                "Person",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                "CreatedOn",
                "Person",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "ModifiedBy",
                "Person",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                "ModifiedOn",
                "Person",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                "RowVersion",
                "Person",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "CreatedBy",
                "Person");

            migrationBuilder.DropColumn(
                "CreatedOn",
                "Person");

            migrationBuilder.DropColumn(
                "ModifiedBy",
                "Person");

            migrationBuilder.DropColumn(
                "ModifiedOn",
                "Person");

            migrationBuilder.DropColumn(
                "RowVersion",
                "Person");
        }
    }
}
