using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NIdentity.Migrations
{
    public partial class M202212181100 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CertificatePermissions",
                columns: table => new
                {
                    No = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    KeySHA1 = table.Column<string>(type: "varchar(41)", maxLength: 41, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccessKeySHA1 = table.Column<string>(type: "varchar(41)", maxLength: 41, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AuthorityKeySHA1 = table.Column<string>(type: "varchar(41)", maxLength: 41, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    LastWriteTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    CanAuthorityInterfere = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanGenerate = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanList = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanRevoke = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanDelete = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanAlter = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificatePermissions", x => x.No);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "BY_ACCSHA1",
                table: "CertificatePermissions",
                column: "AccessKeySHA1");

            migrationBuilder.CreateIndex(
                name: "BY_ATHSHA1",
                table: "CertificatePermissions",
                column: "AuthorityKeySHA1");

            migrationBuilder.CreateIndex(
                name: "BY_KEYSHA1",
                table: "CertificatePermissions",
                column: "KeySHA1");

            migrationBuilder.CreateIndex(
                name: "UK_ANTI_DUP",
                table: "CertificatePermissions",
                columns: new[] { "KeySHA1", "AccessKeySHA1" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CertificatePermissions");
        }
    }
}
