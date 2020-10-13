using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aurora.Insurance.Data.Migrations
{
    public partial class IntroduceAttachments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    AttachmentId = table.Column<Guid>(nullable: false, defaultValueSql: "newsequentialid()"),
                    Title = table.Column<string>(maxLength: 250, nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    FileName = table.Column<string>(maxLength: 250, nullable: true),
                    Url = table.Column<string>(maxLength: 1000, nullable: true),
                    MimeType = table.Column<string>(maxLength: 50, nullable: true),
                    ContentLength = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.AttachmentId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachment");
        }
    }
}
