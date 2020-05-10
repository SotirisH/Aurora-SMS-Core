using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aurora.SMS.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Provider",
                table => new
                {
                    Name = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>("datetime2", nullable: true),
                    LogoUrl = table.Column<string>("nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedOn = table.Column<DateTime>("datetime2", nullable: true),
                    PassWord = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: true),
                    RowVersion = table.Column<byte[]>("rowversion", rowVersion: true, nullable: true),
                    SupportsScheduleMessage = table.Column<bool>("bit", nullable: false),
                    Url = table.Column<string>("nvarchar(255)", maxLength: 255, nullable: false),
                    UserName = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Provider", x => x.Name); });

            migrationBuilder.CreateTable(
                "Template",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>("datetime2", nullable: true),
                    Description = table.Column<string>("nvarchar(255)", maxLength: 255, nullable: true),
                    IsInactive = table.Column<bool>("bit", nullable: false),
                    ModifiedBy = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedOn = table.Column<DateTime>("datetime2", nullable: true),
                    Name = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                    RowVersion = table.Column<byte[]>("rowversion", rowVersion: true, nullable: true),
                    Text = table.Column<string>("nvarchar(max)", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Template", x => x.Id); });

            migrationBuilder.CreateTable(
                "TemplateField",
                table => new
                {
                    Name = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>("datetime2", nullable: true),
                    DataFormat = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>("nvarchar(255)", maxLength: 255, nullable: true),
                    GroupName = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedOn = table.Column<DateTime>("datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>("rowversion", rowVersion: true, nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_TemplateField", x => x.Name); });

            migrationBuilder.CreateTable(
                "SMSHistory",
                table => new
                {
                    Id = table.Column<long>("bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContractId = table.Column<int>("int", nullable: true),
                    CreatedBy = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>("datetime2", nullable: true),
                    Message = table.Column<string>("nvarchar(max)", nullable: false),
                    MobileNumber = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedBy = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedOn = table.Column<DateTime>("datetime2", nullable: true),
                    PersonId = table.Column<int>("int", nullable: true),
                    ProviderFeedBackDateTime = table.Column<DateTime>("datetime2", nullable: true),
                    ProviderFeedback = table.Column<string>("nvarchar(max)", nullable: true),
                    ProviderMsgId = table.Column<string>("nvarchar(255)", maxLength: 255, nullable: true),
                    ProviderName = table.Column<string>("nvarchar(50)", nullable: false),
                    RowVersion = table.Column<byte[]>("rowversion", rowVersion: true, nullable: true),
                    SendDateTime = table.Column<DateTime>("datetime2", nullable: false),
                    SessionId = table.Column<Guid>("uniqueidentifier", nullable: false),
                    SessionName = table.Column<string>("nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<int>("int", nullable: false),
                    TemplateId = table.Column<int>("int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMSHistory", x => x.Id);
                    table.ForeignKey(
                        "FK_SMSHistory_Provider_ProviderName",
                        x => x.ProviderName,
                        "Provider",
                        "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_SMSHistory_Template_TemplateId",
                        x => x.TemplateId,
                        "Template",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_SMSHistory_ProviderName",
                "SMSHistory",
                "ProviderName");

            migrationBuilder.CreateIndex(
                "IX_SMSHistory_TemplateId",
                "SMSHistory",
                "TemplateId");

            migrationBuilder.CreateIndex(
                "IX_Template_Name",
                "Template",
                "Name",
                unique: true);

            // FROM FILE
            string sqlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Migrations\Init.sql");
            migrationBuilder.Sql(File.ReadAllText(sqlFile));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "SMSHistory");

            migrationBuilder.DropTable(
                "TemplateField");

            migrationBuilder.DropTable(
                "Provider");

            migrationBuilder.DropTable(
                "Template");
        }
    }
}
