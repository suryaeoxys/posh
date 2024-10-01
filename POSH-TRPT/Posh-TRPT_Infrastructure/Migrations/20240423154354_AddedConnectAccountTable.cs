using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedConnectAccountTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblStripeCustomers_AspNetUsers_UserId",
                table: "TblStripeCustomers");

            migrationBuilder.DropForeignKey(
                name: "FK_TblStripeCustomersPaymentIntent_TblStripeCustomers_StripeCustomerRecordId",
                table: "TblStripeCustomersPaymentIntent");

            migrationBuilder.DropIndex(
                name: "IX_TblStripeCustomersPaymentIntent_StripeCustomerRecordId",
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

            migrationBuilder.CreateTable(
                name: "TblStripeConnectAccountUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConnectAccountId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Default_Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    External_Accounts_URL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Login_Links_URL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblStripeConnectAccountUsers", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "1bd7fe93-3e76-4342-b3d0-902c08b953f9", new DateTime(2024, 4, 23, 21, 13, 54, 345, DateTimeKind.Local).AddTicks(9606), "AQAAAAEAACcQAAAAEInq9Grho2asjI9bjamIY9qdXMl9ELt6GK+o5zk9araU6of0MRmPnR8+omPjX5VHWQ==", "dbd67089-777c-43a8-9817-92ba18ef20c9", new DateTime(2024, 4, 23, 21, 13, 54, 345, DateTimeKind.Local).AddTicks(9616) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblStripeConnectAccountUsers");

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
                values: new object[] { "0ffe30ea-48f0-4cb4-b801-75b8b859d88c", new DateTime(2024, 4, 22, 19, 45, 55, 297, DateTimeKind.Local).AddTicks(1671), "AQAAAAEAACcQAAAAEOxUJIlbqZRyHKWkLgzlZ5zPI/Yt6Pmm9uVr2W7h1dZaalNdeBnmRsXYsSpb4nEs4Q==", "011d7373-b9ae-412d-a4b6-5d7bb41d2b75", new DateTime(2024, 4, 22, 19, 45, 55, 297, DateTimeKind.Local).AddTicks(1681) });

            migrationBuilder.CreateIndex(
                name: "IX_TblStripeCustomersPaymentIntent_StripeCustomerRecordId",
                table: "TblStripeCustomersPaymentIntent",
                column: "StripeCustomerRecordId");

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
                name: "FK_TblStripeCustomersPaymentIntent_TblStripeCustomers_StripeCustomerRecordId",
                table: "TblStripeCustomersPaymentIntent",
                column: "StripeCustomerRecordId",
                principalTable: "TblStripeCustomers",
                principalColumn: "Id");
        }
    }
}
