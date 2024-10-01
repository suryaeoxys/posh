using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class removedColumnsBool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVehicleRegistrationCompleted",
                table: "TblVehicleDetails");

            migrationBuilder.DropColumn(
                name: "IsAddressRegistrationCompleted",
                table: "TblGeneralAddress");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "263113d9-9d36-4e07-9c7e-54f0d078c9bb", new DateTime(2024, 1, 29, 19, 34, 18, 443, DateTimeKind.Local).AddTicks(4795), "AQAAAAEAACcQAAAAEPtTKqJFXR+m1gO8ujcqnCQ4C6n6CIdm7135gndhL3rUGRmLEFQMZqcYElIy95hEng==", "bb68ffbf-e6a8-4bee-aea6-7342b8b1b4b9", new DateTime(2024, 1, 29, 19, 34, 18, 443, DateTimeKind.Local).AddTicks(4805) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVehicleRegistrationCompleted",
                table: "TblVehicleDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAddressRegistrationCompleted",
                table: "TblGeneralAddress",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "efea7c50-bfb4-4006-b99e-1a2d2c5ad3ec", new DateTime(2024, 1, 26, 5, 11, 42, 149, DateTimeKind.Local).AddTicks(2815), "AQAAAAEAACcQAAAAEAPpFMJeWQVGTf+hckzCCb73GAeGSNAjBTfXpmixoiPBTeonsgPuRNRnjTagxDYntg==", "1751197b-29cc-4ab7-810c-e6ab141a44ac", new DateTime(2024, 1, 26, 5, 11, 42, 149, DateTimeKind.Local).AddTicks(2826) });
        }
    }
}
