using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NIdentity.Migrations
{
    public partial class M202212131206 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RevokationInventories",
                columns: table => new
                {
                    KeySHA1 = table.Column<string>(type: "varchar(41)", maxLength: 41, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Subject = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KeyIdentifier = table.Column<string>(type: "varchar(41)", maxLength: 41, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    LastWriteTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Revision = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevokationInventories", x => x.KeySHA1);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Revokations",
                columns: table => new
                {
                    Number = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AuthorityKeySHA1 = table.Column<string>(type: "varchar(41)", maxLength: 41, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RefSHA1 = table.Column<string>(type: "varchar(41)", maxLength: 41, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Revision = table.Column<long>(type: "bigint", nullable: false),
                    SerialNumber = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IssuerKeyIdentifier = table.Column<string>(type: "varchar(41)", maxLength: 41, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Time = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Reason = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Revokations", x => x.Number);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "BY_ISSUER1",
                table: "Revokations",
                column: "AuthorityKeySHA1");

            migrationBuilder.CreateIndex(
                name: "BY_REV",
                table: "Revokations",
                column: "Revision");

            migrationBuilder.CreateIndex(
                name: "UK_RF3",
                table: "Revokations",
                columns: new[] { "AuthorityKeySHA1", "RefSHA1" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RevokationInventories");

            migrationBuilder.DropTable(
                name: "Revokations");
        }
    }
}
