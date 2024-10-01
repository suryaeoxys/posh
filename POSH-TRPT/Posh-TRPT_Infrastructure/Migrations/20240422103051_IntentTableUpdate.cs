using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class IntentTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "StripeCustomerId",
                table: "TblStripeCustomersPaymentIntent",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "9e5950f8-15e3-42b4-abed-d27144435bf8", new DateTime(2024, 4, 22, 16, 0, 50, 907, DateTimeKind.Local).AddTicks(339), "AQAAAAEAACcQAAAAEEYaF+FQWGmBltLndn/ZDFVBtgmbLyU/VETYLVw1iEnDLflMgRouvpawp7tyxGXJjA==", "bb0dbdf0-b510-4234-9531-9b82f7b59e66", new DateTime(2024, 4, 22, 16, 0, 50, 907, DateTimeKind.Local).AddTicks(348) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StripeCustomerId",
                table: "TblStripeCustomersPaymentIntent",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "7f158fb7-eb22-4f05-b670-f506be4a0a7b", new DateTime(2024, 4, 22, 15, 53, 37, 612, DateTimeKind.Local).AddTicks(2846), "AQAAAAEAACcQAAAAEIVO+u+9VyITrY2NPXGogDWD6sD7WEdbKV6TFt3ImsxW3QyvpfD5b2R1iorabhYz6w==", "5454c183-ed4d-4583-95b5-8cf34c2933d9", new DateTime(2024, 4, 22, 15, 53, 37, 612, DateTimeKind.Local).AddTicks(2857) });
        }
    }
}
