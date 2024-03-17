using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehiGate.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Checkings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SiteId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DriverId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VehicleId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TankId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CheckingInDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckingOutDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BLNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntranceInspectorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExistInspectorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checkings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Checkings_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Checkings_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Checkings_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Checkings_Vehicles_TankId",
                        column: x => x.TankId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Checkings_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Checkings_CustomerId",
                table: "Checkings",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Checkings_DriverId",
                table: "Checkings",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Checkings_SiteId",
                table: "Checkings",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Checkings_TankId",
                table: "Checkings",
                column: "TankId");

            migrationBuilder.CreateIndex(
                name: "IX_Checkings_VehicleId",
                table: "Checkings",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Checkings");
        }
    }
}
