using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehiGate.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class changeCheckListConception : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriverInspectionChecklists_Checklists_ChecklistId",
                table: "DriverInspectionChecklists");

            migrationBuilder.DropForeignKey(
                name: "FK_DriverInspectionChecklists_DriverInspections_DriverInspectionId",
                table: "DriverInspectionChecklists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DriverInspectionChecklists",
                table: "DriverInspectionChecklists");

            migrationBuilder.DropIndex(
                name: "IX_DriverInspectionChecklists_DriverInspectionId",
                table: "DriverInspectionChecklists");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "VehicleInspectionChecklists");

            migrationBuilder.DropColumn(
                name: "State",
                table: "VehicleInspectionChecklists");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "DriverInspectionChecklists");

            migrationBuilder.DropColumn(
                name: "State",
                table: "DriverInspectionChecklists");

            migrationBuilder.DropColumn(
                name: "AssociatedTo",
                table: "Checklists");

            migrationBuilder.AddColumn<string>(
                name: "ChecklistId",
                table: "VehicleInspections",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "DriverInspections",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChecklistId",
                table: "DriverInspections",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DriverInspectionId",
                table: "DriverInspectionChecklists",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ChecklistId",
                table: "DriverInspectionChecklists",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "DriverInspectionChecklists",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Checklists",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DriverInspectionChecklists",
                table: "DriverInspectionChecklists",
                columns: new[] { "DriverInspectionId", "ChecklistId" });

            migrationBuilder.CreateTable(
                name: "CheckItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CheckListItems",
                columns: table => new
                {
                    CheckListId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CheckItemId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckListItems", x => new { x.CheckListId, x.CheckItemId });
                    table.ForeignKey(
                        name: "FK_CheckListItems_CheckItems_CheckItemId",
                        column: x => x.CheckItemId,
                        principalTable: "CheckItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckListItems_Checklists_CheckListId",
                        column: x => x.CheckListId,
                        principalTable: "Checklists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspections_ChecklistId",
                table: "VehicleInspections",
                column: "ChecklistId",
                unique: true,
                filter: "[ChecklistId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DriverInspections_ChecklistId",
                table: "DriverInspections",
                column: "ChecklistId",
                unique: true,
                filter: "[ChecklistId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CheckListItems_CheckItemId",
                table: "CheckListItems",
                column: "CheckItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_DriverInspectionChecklists_Checklists_ChecklistId",
                table: "DriverInspectionChecklists",
                column: "ChecklistId",
                principalTable: "Checklists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DriverInspectionChecklists_DriverInspections_DriverInspectionId",
                table: "DriverInspectionChecklists",
                column: "DriverInspectionId",
                principalTable: "DriverInspections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DriverInspections_Checklists_ChecklistId",
                table: "DriverInspections",
                column: "ChecklistId",
                principalTable: "Checklists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleInspections_Checklists_ChecklistId",
                table: "VehicleInspections",
                column: "ChecklistId",
                principalTable: "Checklists",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriverInspectionChecklists_Checklists_ChecklistId",
                table: "DriverInspectionChecklists");

            migrationBuilder.DropForeignKey(
                name: "FK_DriverInspectionChecklists_DriverInspections_DriverInspectionId",
                table: "DriverInspectionChecklists");

            migrationBuilder.DropForeignKey(
                name: "FK_DriverInspections_Checklists_ChecklistId",
                table: "DriverInspections");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleInspections_Checklists_ChecklistId",
                table: "VehicleInspections");

            migrationBuilder.DropTable(
                name: "CheckListItems");

            migrationBuilder.DropTable(
                name: "CheckItems");

            migrationBuilder.DropIndex(
                name: "IX_VehicleInspections_ChecklistId",
                table: "VehicleInspections");

            migrationBuilder.DropIndex(
                name: "IX_DriverInspections_ChecklistId",
                table: "DriverInspections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DriverInspectionChecklists",
                table: "DriverInspectionChecklists");

            migrationBuilder.DropColumn(
                name: "ChecklistId",
                table: "VehicleInspections");

            migrationBuilder.DropColumn(
                name: "ChecklistId",
                table: "DriverInspections");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "VehicleInspectionChecklists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "State",
                table: "VehicleInspectionChecklists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "DriverInspections",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "DriverInspectionChecklists",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ChecklistId",
                table: "DriverInspectionChecklists",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "DriverInspectionId",
                table: "DriverInspectionChecklists",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "DriverInspectionChecklists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "State",
                table: "DriverInspectionChecklists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Checklists",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<int>(
                name: "AssociatedTo",
                table: "Checklists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DriverInspectionChecklists",
                table: "DriverInspectionChecklists",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DriverInspectionChecklists_DriverInspectionId",
                table: "DriverInspectionChecklists",
                column: "DriverInspectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_DriverInspectionChecklists_Checklists_ChecklistId",
                table: "DriverInspectionChecklists",
                column: "ChecklistId",
                principalTable: "Checklists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DriverInspectionChecklists_DriverInspections_DriverInspectionId",
                table: "DriverInspectionChecklists",
                column: "DriverInspectionId",
                principalTable: "DriverInspections",
                principalColumn: "Id");
        }
    }
}
