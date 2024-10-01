using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class newolumnsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Payment_Method_Types",
                table: "TblStripeCustomersSetupIntent");

            migrationBuilder.AddColumn<string>(
                name: "DefaultPaymentMethodId",
                table: "TblBookingDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                table: "TblBookingDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "TblBookingDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "515658e7-81e3-40b9-a9ec-9213d02a454f", new DateTime(2024, 5, 2, 8, 45, 49, 977, DateTimeKind.Local).AddTicks(4658), "AQAAAAEAACcQAAAAEKGIGbeEYSotqqYR3+I0vMlIhznPHFLjNMStEN4OykZaX62rJFKwRh8UiWc/4p4fJw==", "2398a114-bce1-4124-928e-631303b4db3c", new DateTime(2024, 5, 2, 8, 45, 49, 977, DateTimeKind.Local).AddTicks(4671) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultPaymentMethodId",
                table: "TblBookingDetail");

            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                table: "TblBookingDetail");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "TblBookingDetail");

            migrationBuilder.AddColumn<string>(
                name: "Payment_Method_Types",
                table: "TblStripeCustomersSetupIntent",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "74c209eb-105d-4935-9928-f0ba4c4231bb", new DateTime(2024, 4, 26, 19, 43, 33, 410, DateTimeKind.Local).AddTicks(5246), "AQAAAAEAACcQAAAAEBW42wlN56qUaSKKst4Z/V4t6KNWXvbb6RtW+EtZjpOyybMtUnRprdT1ZSTZpfy4cA==", "926909d3-62bb-4b43-a53c-85d64f262cfd", new DateTime(2024, 4, 26, 19, 43, 33, 410, DateTimeKind.Local).AddTicks(5256) });
        }
    }
}
