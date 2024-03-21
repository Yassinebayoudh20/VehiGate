using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehiGate.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckListAssociationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Checklists",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssociatedTo = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checklists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DriverInspectionChecklists",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DriverInspectionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ChecklistId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    State = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverInspectionChecklists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DriverInspectionChecklists_Checklists_ChecklistId",
                        column: x => x.ChecklistId,
                        principalTable: "Checklists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DriverInspectionChecklists_DriverInspections_DriverInspectionId",
                        column: x => x.DriverInspectionId,
                        principalTable: "DriverInspections",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VehicleInspectionChecklists",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VehicleInspectionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ChecklistId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    State = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleInspectionChecklists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleInspectionChecklists_Checklists_ChecklistId",
                        column: x => x.ChecklistId,
                        principalTable: "Checklists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VehicleInspectionChecklists_VehicleInspections_VehicleInspectionId",
                        column: x => x.VehicleInspectionId,
                        principalTable: "VehicleInspections",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverInspectionChecklists_ChecklistId",
                table: "DriverInspectionChecklists",
                column: "ChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverInspectionChecklists_DriverInspectionId",
                table: "DriverInspectionChecklists",
                column: "DriverInspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspectionChecklists_ChecklistId",
                table: "VehicleInspectionChecklists",
                column: "ChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspectionChecklists_VehicleInspectionId",
                table: "VehicleInspectionChecklists",
                column: "VehicleInspectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverInspectionChecklists");

            migrationBuilder.DropTable(
                name: "VehicleInspectionChecklists");

            migrationBuilder.DropTable(
                name: "Checklists");
        }
    }
}
