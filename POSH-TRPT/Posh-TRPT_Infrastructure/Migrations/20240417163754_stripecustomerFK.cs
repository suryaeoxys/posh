using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class stripecustomerFK : Migration
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
                values: new object[] { "3564806f-63fe-46ad-bd23-aec8ac70eb31", new DateTime(2024, 4, 17, 22, 7, 54, 468, DateTimeKind.Local).AddTicks(4336), "AQAAAAEAACcQAAAAEPG6lDLkIIUYdJpTsfRP4hnoMq8CenNx88NU8VKu+mkdY5EpMcHubrypxcacg4sTxg==", "87e6f48a-927f-4421-8bfd-884eb2484695", new DateTime(2024, 4, 17, 22, 7, 54, 468, DateTimeKind.Local).AddTicks(4354) });

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblStripeCustomers_AspNetUsers_UserId",
                table: "TblStripeCustomers");

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
                values: new object[] { "2639de4e-5227-4ffb-80a5-ecc8d75334ee", new DateTime(2024, 4, 17, 21, 30, 58, 830, DateTimeKind.Local).AddTicks(8297), "AQAAAAEAACcQAAAAEAtM8wmzejseZ2aAuK+7ATh+YBsRmY3CuZzgZ3QfhVbOvY4L5ZVrDpRJMXstXYouwg==", "8bf71658-fe73-49dd-9713-bc00265790e2", new DateTime(2024, 4, 17, 21, 30, 58, 830, DateTimeKind.Local).AddTicks(8309) });
        }
    }
}
