using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NIdentity.Migrations
{
    public partial class M202212121026 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CertificateDocuments",
                columns: table => new
                {
                    KeySHA1 = table.Column<string>(type: "varchar(41)", maxLength: 41, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PathName = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RefSHA1 = table.Column<string>(type: "varchar(41)", maxLength: 41, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentPathName = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HasChildren = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Access = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    LastWriteTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    RevisionNumber = table.Column<long>(type: "bigint", nullable: false),
                    MimeType = table.Column<string>(type: "varchar(48)", maxLength: 48, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Data = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ID", x => new { x.KeySHA1, x.PathName });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    KeySHA1 = table.Column<string>(type: "varchar(41)", maxLength: 41, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RefSHA1 = table.Column<string>(type: "varchar(41)", maxLength: 41, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KeyIdentifier = table.Column<string>(type: "varchar(41)", maxLength: 41, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Subject = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubjectHash = table.Column<string>(type: "varchar(41)", maxLength: 41, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IssuerKeyIdentifier = table.Column<string>(type: "varchar(41)", maxLength: 41, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Issuer = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IssuerHash = table.Column<string>(type: "varchar(41)", maxLength: 41, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SerialNumber = table.Column<string>(type: "varchar(65)", maxLength: 65, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    ExpirationTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    RevokeReason = table.Column<int>(type: "int", nullable: false),
                    RevokeTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Thumbprint = table.Column<string>(type: "varchar(41)", maxLength: 41, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ID", x => x.KeySHA1);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CertificateStores",
                columns: table => new
                {
                    KeySHA1 = table.Column<string>(type: "varchar(41)", maxLength: 41, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RefSHA1 = table.Column<string>(type: "varchar(41)", maxLength: 41, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Base64 = table.Column<string>(type: "LONGTEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ID", x => x.KeySHA1);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "BY_ACCESS",
                table: "CertificateDocuments",
                column: "Access");

            migrationBuilder.CreateIndex(
                name: "BY_CTIME1",
                table: "CertificateDocuments",
                column: "CreationTime");

            migrationBuilder.CreateIndex(
                name: "BY_MIME_TYPE",
                table: "CertificateDocuments",
                column: "MimeType");

            migrationBuilder.CreateIndex(
                name: "BY_PARENT",
                table: "CertificateDocuments",
                column: "ParentPathName");

            migrationBuilder.CreateIndex(
                name: "BY_WCODE",
                table: "CertificateDocuments",
                column: "LastWriteTime");

            migrationBuilder.CreateIndex(
                name: "UK_RF1",
                table: "CertificateDocuments",
                columns: new[] { "RefSHA1", "PathName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "BY_CTIME",
                table: "Certificates",
                column: "CreationTime");

            migrationBuilder.CreateIndex(
                name: "BY_ISSUER",
                table: "Certificates",
                column: "Issuer");

            migrationBuilder.CreateIndex(
                name: "BY_ISSUER_HASH",
                table: "Certificates",
                column: "IssuerHash");

            migrationBuilder.CreateIndex(
                name: "BY_ISSUER_KEY",
                table: "Certificates",
                column: "IssuerKeyIdentifier");

            migrationBuilder.CreateIndex(
                name: "BY_KEY",
                table: "Certificates",
                column: "KeyIdentifier");

            migrationBuilder.CreateIndex(
                name: "BY_RCODE",
                table: "Certificates",
                column: "RevokeReason");

            migrationBuilder.CreateIndex(
                name: "BY_RTIME",
                table: "Certificates",
                column: "RevokeTime");

            migrationBuilder.CreateIndex(
                name: "BY_SERIAL_NUMBER",
                table: "Certificates",
                column: "SerialNumber");

            migrationBuilder.CreateIndex(
                name: "BY_SUBJECT",
                table: "Certificates",
                column: "Subject");

            migrationBuilder.CreateIndex(
                name: "BY_SUBJECT_HASH",
                table: "Certificates",
                column: "SubjectHash");

            migrationBuilder.CreateIndex(
                name: "BY_XTIME",
                table: "Certificates",
                column: "ExpirationTime");

            migrationBuilder.CreateIndex(
                name: "UK_RF",
                table: "Certificates",
                column: "RefSHA1",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_SHK",
                table: "Certificates",
                columns: new[] { "SubjectHash", "KeyIdentifier" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_SIH",
                table: "Certificates",
                columns: new[] { "SerialNumber", "IssuerHash" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_SKI",
                table: "Certificates",
                columns: new[] { "SerialNumber", "IssuerKeyIdentifier" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_SNK",
                table: "Certificates",
                columns: new[] { "Subject", "KeyIdentifier" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_RF2",
                table: "CertificateStores",
                column: "RefSHA1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CertificateDocuments");

            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DropTable(
                name: "CertificateStores");
        }
    }
}
