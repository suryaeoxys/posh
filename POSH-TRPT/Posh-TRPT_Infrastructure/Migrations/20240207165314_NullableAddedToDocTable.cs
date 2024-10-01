using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class NullableAddedToDocTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VehicleRegistrationDocName",
                table: "TblDriverDocuments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "InsuarnceDocName",
                table: "TblDriverDocuments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DrivingLicenceDocName",
                table: "TblDriverDocuments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "248c77d3-d0da-467b-b144-dec0eccbc5f4", new DateTime(2024, 2, 7, 22, 23, 13, 20, DateTimeKind.Local).AddTicks(9369), "AQAAAAEAACcQAAAAEP7L3e7amzTaCxRQpdcmN1RYdgw42gNwM+8vdHPtyMjMea686eLh3f8upO5a+l4VzA==", "2e8938f0-fac7-4c3e-96ad-949cad5a485c", new DateTime(2024, 2, 7, 22, 23, 13, 20, DateTimeKind.Local).AddTicks(9389) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VehicleRegistrationDocName",
                table: "TblDriverDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InsuarnceDocName",
                table: "TblDriverDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DrivingLicenceDocName",
                table: "TblDriverDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "5eff80ec-1d82-4c07-ae43-274450246fe7", new DateTime(2024, 2, 6, 11, 35, 37, 467, DateTimeKind.Local).AddTicks(3108), "AQAAAAEAACcQAAAAECQw+vr7mxiWRHFh3Fgi/+tvrjp3hH+4qoX9IJmJxQZ3M85bkHZB2u9Ov65a6uaoVA==", "55ea2ead-d503-4811-86ef-26e60adf6391", new DateTime(2024, 2, 6, 11, 35, 37, 467, DateTimeKind.Local).AddTicks(3119) });
        }
    }
}
