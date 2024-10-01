using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class ModifiedAddColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "TblStripeCustomersPaymentIntent",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "0f8c5b6b-22fc-4bc7-8e57-bff12cc2bd61", new DateTime(2024, 4, 22, 19, 11, 8, 70, DateTimeKind.Local).AddTicks(1099), "AQAAAAEAACcQAAAAEHi2XM09WecTc8dYGitBEUzXraSXTY0I3qezd7o0WsypGMeuRios7f4tf9rc7zxOvg==", "1f091f0c-9e02-46e4-b13b-73c8e5823a4c", new DateTime(2024, 4, 22, 19, 11, 8, 70, DateTimeKind.Local).AddTicks(1108) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "TblStripeCustomersPaymentIntent");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "ce40d322-fa05-42aa-8e7b-f2ae8ba09965", new DateTime(2024, 4, 22, 19, 9, 54, 826, DateTimeKind.Local).AddTicks(7105), "AQAAAAEAACcQAAAAEBL7N9T5ShTtjNYrNIQGi8/Mo1xg5a4/ONnr7pkFHXkP7deVTpXwi7Uzoyhbmrnw6Q==", "a33b814b-2c4f-4b1f-957d-2106ef1fb720", new DateTime(2024, 4, 22, 19, 9, 54, 826, DateTimeKind.Local).AddTicks(7118) });
        }
    }
}
