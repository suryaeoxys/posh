using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class addnewcolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<bool>(
                name: "IsDocUploadCompleted",
                table: "TblDriverDocuments",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVehicleRegistrationCompleted",
                table: "TblVehicleDetails");

            migrationBuilder.DropColumn(
                name: "IsAddressRegistrationCompleted",
                table: "TblGeneralAddress");

            migrationBuilder.DropColumn(
                name: "IsDocUploadCompleted",
                table: "TblDriverDocuments");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "2ee75b9d-bf7f-4adf-b335-52905d9fca44", new DateTime(2024, 1, 26, 3, 35, 27, 954, DateTimeKind.Local).AddTicks(979), "AQAAAAEAACcQAAAAELOdJFuX26Br6w2j1XQTP5+YoHzXOAJzWaNfCgttJ11Gf0CnbCM3CaQKzjKm8MQoRg==", "9298dfcd-8dd3-404a-b421-13fdd28ce9f1", new DateTime(2024, 1, 26, 3, 35, 27, 954, DateTimeKind.Local).AddTicks(991) });
        }
    }
}
