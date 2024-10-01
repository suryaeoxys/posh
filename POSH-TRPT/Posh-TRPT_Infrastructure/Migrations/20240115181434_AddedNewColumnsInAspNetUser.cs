using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedNewColumnsInAspNetUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DrivingLicenceNumber",
                table: "TblDriverDocuments");

            migrationBuilder.DropColumn(
                name: "InsuarnceNumber",
                table: "TblDriverDocuments");

            migrationBuilder.DropColumn(
                name: "PassportNumber",
                table: "TblDriverDocuments");

            migrationBuilder.AlterColumn<string>(
                name: "InsuarnceDocPhoto",
                table: "TblDriverDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DrivingLicencePhoto",
                table: "TblDriverDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleRegistrationPhoto",
                table: "TblDriverDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDocumentVerified",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "489d3254-5bb5-448a-994b-6adf8a8bf882", new DateTime(2024, 1, 15, 23, 44, 34, 151, DateTimeKind.Local).AddTicks(254), "AQAAAAEAACcQAAAAEI4gxgnlTmGlhdpj9Nat8XrON93jb8EkmeWeCt7rpAgbPIlEo9WepNOb+ej0l8d6Ug==", "d6ae4a62-a400-42c9-bb07-1afbc1b6e0d8", new DateTime(2024, 1, 15, 23, 44, 34, 151, DateTimeKind.Local).AddTicks(263) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehicleRegistrationPhoto",
                table: "TblDriverDocuments");

            migrationBuilder.DropColumn(
                name: "IsDocumentVerified",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "InsuarnceDocPhoto",
                table: "TblDriverDocuments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DrivingLicencePhoto",
                table: "TblDriverDocuments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "DrivingLicenceNumber",
                table: "TblDriverDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsuarnceNumber",
                table: "TblDriverDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PassportNumber",
                table: "TblDriverDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "1a2e516a-5dcd-43b5-a8cb-2ad8c612ac58", new DateTime(2024, 1, 12, 20, 52, 5, 540, DateTimeKind.Local).AddTicks(9857), "AQAAAAEAACcQAAAAEOZz6LJVxBdrqQtEhJ/YhCFjKBCO645eENP8Psw4NyCCB9o9U3zKdcPC4DPAtuulJA==", "238457ae-7180-4d90-928e-7126d5da8b0c", new DateTime(2024, 1, 12, 20, 52, 5, 540, DateTimeKind.Local).AddTicks(9866) });
        }
    }
}
