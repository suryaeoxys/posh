using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class IntentTableUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TblStripeCustomers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "b1cebafa-b81b-4ea0-af36-53686ac31cb7", new DateTime(2024, 4, 22, 16, 2, 21, 744, DateTimeKind.Local).AddTicks(5260), "AQAAAAEAACcQAAAAEIiRYJLssroLOLklqFuKCEklgLz/xRIW5YGU6Ds2K/CNXMYFZ2THE6YZ2qsfbUJ3lg==", "a9ba6668-5e99-4506-a488-afc1358fd35f", new DateTime(2024, 4, 22, 16, 2, 21, 744, DateTimeKind.Local).AddTicks(5274) });

            migrationBuilder.CreateIndex(
                name: "IX_TblStripeCustomersPaymentIntent_StripeCustomerId",
                table: "TblStripeCustomersPaymentIntent",
                column: "StripeCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TblStripeCustomers_UserId",
                table: "TblStripeCustomers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TblStripeCustomers_AspNetUsers_UserId",
                table: "TblStripeCustomers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TblStripeCustomersPaymentIntent_TblStripeCustomers_StripeCustomerId",
                table: "TblStripeCustomersPaymentIntent",
                column: "StripeCustomerId",
                principalTable: "TblStripeCustomers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblStripeCustomers_AspNetUsers_UserId",
                table: "TblStripeCustomers");

            migrationBuilder.DropForeignKey(
                name: "FK_TblStripeCustomersPaymentIntent_TblStripeCustomers_StripeCustomerId",
                table: "TblStripeCustomersPaymentIntent");

            migrationBuilder.DropIndex(
                name: "IX_TblStripeCustomersPaymentIntent_StripeCustomerId",
                table: "TblStripeCustomersPaymentIntent");

            migrationBuilder.DropIndex(
                name: "IX_TblStripeCustomers_UserId",
                table: "TblStripeCustomers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TblStripeCustomers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "9e5950f8-15e3-42b4-abed-d27144435bf8", new DateTime(2024, 4, 22, 16, 0, 50, 907, DateTimeKind.Local).AddTicks(339), "AQAAAAEAACcQAAAAEEYaF+FQWGmBltLndn/ZDFVBtgmbLyU/VETYLVw1iEnDLflMgRouvpawp7tyxGXJjA==", "bb0dbdf0-b510-4234-9531-9b82f7b59e66", new DateTime(2024, 4, 22, 16, 0, 50, 907, DateTimeKind.Local).AddTicks(348) });
        }
    }
}
