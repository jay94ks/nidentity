using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NIdentity.Migrations
{
    public partial class M202212161352 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EndpointInventories",
                columns: table => new
                {
                    Identity = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Owner = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OwnerKeyIdentifier = table.Column<string>(type: "varchar(41)", maxLength: 41, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OwnerKeySHA1 = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsPublic = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsMetadataPublic = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    LastUpdateTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    CautionTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    CautionLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndpointInventories", x => x.Identity);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EndpointNetworks",
                columns: table => new
                {
                    Inventory = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubnetMask = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CautionTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    CautionLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EP", x => new { x.Inventory, x.Address, x.SubnetMask });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Endpoints",
                columns: table => new
                {
                    Inventory = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CautionTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    CautionLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EP", x => new { x.Inventory, x.Address });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "BY_CAUTION1",
                table: "EndpointInventories",
                column: "CautionLevel");

            migrationBuilder.CreateIndex(
                name: "BY_NAME1",
                table: "EndpointInventories",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "BY_OWNER",
                table: "EndpointInventories",
                column: "Owner");

            migrationBuilder.CreateIndex(
                name: "BY_OWNER_KEY_ID",
                table: "EndpointInventories",
                column: "OwnerKeyIdentifier");

            migrationBuilder.CreateIndex(
                name: "BY_PUBLIC",
                table: "EndpointInventories",
                column: "IsPublic");

            migrationBuilder.CreateIndex(
                name: "BY_ADDR1",
                table: "EndpointNetworks",
                column: "Address");

            migrationBuilder.CreateIndex(
                name: "BY_CAUTION2",
                table: "EndpointNetworks",
                column: "CautionLevel");

            migrationBuilder.CreateIndex(
                name: "BY_NAME2",
                table: "EndpointNetworks",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "BY_TYPE1",
                table: "EndpointNetworks",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "BY_ADDR",
                table: "Endpoints",
                column: "Address");

            migrationBuilder.CreateIndex(
                name: "BY_CAUTION",
                table: "Endpoints",
                column: "CautionLevel");

            migrationBuilder.CreateIndex(
                name: "BY_NAME",
                table: "Endpoints",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "BY_TYPE",
                table: "Endpoints",
                column: "Type");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EndpointInventories");

            migrationBuilder.DropTable(
                name: "EndpointNetworks");

            migrationBuilder.DropTable(
                name: "Endpoints");
        }
    }
}
