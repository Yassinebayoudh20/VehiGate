using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehiGate.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorizationPropsToDriverAndVehcile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AuthorizedFrom",
                table: "Vehicles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "AuthorizedTo",
                table: "Vehicles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsAuthorized",
                table: "Vehicles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAuthorized",
                table: "VehicleInspections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "AuthorizedFrom",
                table: "Drivers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "AuthorizedTo",
                table: "Drivers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsAuthorized",
                table: "Drivers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAuthorized",
                table: "DriverInspections",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorizedFrom",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "AuthorizedTo",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "IsAuthorized",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "IsAuthorized",
                table: "VehicleInspections");

            migrationBuilder.DropColumn(
                name: "AuthorizedFrom",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "AuthorizedTo",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "IsAuthorized",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "IsAuthorized",
                table: "DriverInspections");
        }
    }
}
