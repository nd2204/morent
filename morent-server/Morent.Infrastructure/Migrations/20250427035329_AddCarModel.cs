using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Morent.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCarModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DropoffAddress",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "DropoffLatitude",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "DropoffLocation_City",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "DropoffLocation_Country",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "DropoffLocation_State",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "DropoffLocation_ZipCode",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "DropoffLongitude",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "PickupLatitude",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "PickupLocation_State",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "PickupLocation_ZipCode",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "PickupLongitude",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "CurrentLocation_Latitude",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "CurrentLocation_Longitude",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "CurrentLocation_State",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "FuelType",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "PickupLocation_Country",
                table: "Rentals",
                newName: "PickupCountry");

            migrationBuilder.RenameColumn(
                name: "PickupLocation_City",
                table: "Rentals",
                newName: "PickupCity");

            migrationBuilder.RenameColumn(
                name: "CurrentLocation_ZipCode",
                table: "Cars",
                newName: "CarModelId");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldMaxLength: 20);

            migrationBuilder.CreateTable(
                name: "CarModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Brand = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ModelName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    FuelType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Gearbox = table.Column<int>(type: "INTEGER", maxLength: 50, nullable: false),
                    FuelTankCapacity = table.Column<int>(type: "INTEGER", nullable: false),
                    SeatCapacity = table.Column<int>(type: "INTEGER", nullable: false),
                    CarType = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarModels", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_CarModelId",
                table: "Cars",
                column: "CarModelId");

            migrationBuilder.CreateIndex(
                name: "IX_CarModels_Brand_ModelName_Year",
                table: "CarModels",
                columns: new[] { "Brand", "ModelName", "Year" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarModels_CarModelId",
                table: "Cars",
                column: "CarModelId",
                principalTable: "CarModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarModels_CarModelId",
                table: "Cars");

            migrationBuilder.DropTable(
                name: "CarModels");

            migrationBuilder.DropIndex(
                name: "IX_Cars_CarModelId",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "PickupCountry",
                table: "Rentals",
                newName: "PickupLocation_Country");

            migrationBuilder.RenameColumn(
                name: "PickupCity",
                table: "Rentals",
                newName: "PickupLocation_City");

            migrationBuilder.RenameColumn(
                name: "CarModelId",
                table: "Cars",
                newName: "CurrentLocation_ZipCode");

            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "Users",
                type: "INTEGER",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<string>(
                name: "DropoffAddress",
                table: "Rentals",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "DropoffLatitude",
                table: "Rentals",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DropoffLocation_City",
                table: "Rentals",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DropoffLocation_Country",
                table: "Rentals",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DropoffLocation_State",
                table: "Rentals",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DropoffLocation_ZipCode",
                table: "Rentals",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "DropoffLongitude",
                table: "Rentals",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PickupLatitude",
                table: "Rentals",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PickupLocation_State",
                table: "Rentals",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PickupLocation_ZipCode",
                table: "Rentals",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "PickupLongitude",
                table: "Rentals",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Cars",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Cars",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "CurrentLocation_Latitude",
                table: "Cars",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CurrentLocation_Longitude",
                table: "Cars",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentLocation_State",
                table: "Cars",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FuelType",
                table: "Cars",
                type: "INTEGER",
                maxLength: 30,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Cars",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Cars",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
