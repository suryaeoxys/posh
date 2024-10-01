using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class changedTypeStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TblStripeCustomersPaymentIntent",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Payment_Method",
                table: "TblStripeCustomersPaymentIntent",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "f5846442-77e6-4389-9faf-8999bdcf37a5", new DateTime(2024, 4, 22, 17, 24, 6, 832, DateTimeKind.Local).AddTicks(2755), "AQAAAAEAACcQAAAAEGgCEc5EkkfSWxxoM2og2e1uh0FyzbHdVtzSvZvZsfcJzJOEVw0ua/0HyAttzxFRAg==", "85bd3c98-6b51-4f3b-8a80-60eae64f54ae", new DateTime(2024, 4, 22, 17, 24, 6, 832, DateTimeKind.Local).AddTicks(2764) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "TblStripeCustomersPaymentIntent",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Payment_Method",
                table: "TblStripeCustomersPaymentIntent",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "7c6caf88-ab04-437f-9a3a-f2855a7c784b", new DateTime(2024, 4, 22, 17, 3, 1, 288, DateTimeKind.Local).AddTicks(144), "AQAAAAEAACcQAAAAEPmLe7tLMg6+UsEj6rnxzIrkTUP41UWFTYVBnrmFFRbaF0Vw6ylHlBr7VUhCEP0dZg==", "a9b7a832-88ef-4754-9c0d-c233b6737199", new DateTime(2024, 4, 22, 17, 3, 1, 288, DateTimeKind.Local).AddTicks(156) });
        }
    }
}
