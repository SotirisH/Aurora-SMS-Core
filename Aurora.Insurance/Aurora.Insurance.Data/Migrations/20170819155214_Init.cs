using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aurora.Insurance.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Company",
                table => new
                {
                    Id = table.Column<string>("nvarchar(7)", maxLength: 7, nullable: false),
                    CreatedBy = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>("datetime2", nullable: true),
                    Description = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedOn = table.Column<DateTime>("datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>("rowversion", rowVersion: true, nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Company", x => x.Id); });

            migrationBuilder.CreateTable(
                "Person",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>("nvarchar(250)", maxLength: 250, nullable: true),
                    BirthDate = table.Column<DateTime>("datetime2", nullable: true),
                    DrivingLicenceNum = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: true),
                    FatherName = table.Column<string>("nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                    TaxId = table.Column<string>("nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>("nvarchar(12)", maxLength: 12, nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Person", x => x.Id); });

            migrationBuilder.CreateTable(
                "Contract",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CanceledDate = table.Column<DateTime>("datetime2", nullable: true),
                    CompanyId = table.Column<string>("nvarchar(7)", nullable: true),
                    ContractNumber = table.Column<string>("nvarchar(15)", maxLength: 15, nullable: false),
                    ExpireDate = table.Column<DateTime>("datetime2", nullable: false),
                    GrossAmount = table.Column<decimal>("decimal(18, 2)", nullable: false),
                    IsCanceled = table.Column<bool>("bit", nullable: false),
                    IssueDate = table.Column<DateTime>("datetime2", nullable: false),
                    NetAmount = table.Column<decimal>("decimal(18, 2)", nullable: false),
                    PersonId = table.Column<int>("int", nullable: false),
                    PlateNumber = table.Column<string>("nvarchar(15)", maxLength: 15, nullable: false),
                    ReceiptNumber = table.Column<string>("nvarchar(15)", maxLength: 15, nullable: false),
                    StartDate = table.Column<DateTime>("datetime2", nullable: false),
                    TaxAmount = table.Column<decimal>("decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract", x => x.Id);
                    table.ForeignKey(
                        "FK_Contract_Company_CompanyId",
                        x => x.CompanyId,
                        "Company",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Contract_Person_PersonId",
                        x => x.PersonId,
                        "Person",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Phone",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Number = table.Column<string>("nvarchar(50)", maxLength: 50, nullable: false),
                    PersonId = table.Column<int>("int", nullable: false),
                    PhoneType = table.Column<int>("int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phone", x => x.Id);
                    table.ForeignKey(
                        "FK_Phone_Person_PersonId",
                        x => x.PersonId,
                        "Person",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_Contract_CompanyId",
                "Contract",
                "CompanyId");

            migrationBuilder.CreateIndex(
                "IX_Contract_PersonId",
                "Contract",
                "PersonId");

            migrationBuilder.CreateIndex(
                "IX_Contract_ContractNumber_ReceiptNumber",
                "Contract",
                new[]
                {
                    "ContractNumber",
                    "ReceiptNumber"
                });

            migrationBuilder.CreateIndex(
                "IX_Phone_PersonId",
                "Phone",
                "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Contract");

            migrationBuilder.DropTable(
                "Phone");

            migrationBuilder.DropTable(
                "Company");

            migrationBuilder.DropTable(
                "Person");
        }
    }
}
