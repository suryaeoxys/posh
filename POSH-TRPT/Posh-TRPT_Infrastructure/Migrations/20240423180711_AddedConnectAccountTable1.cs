using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedConnectAccountTable1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Payouts_Enabled",
                table: "TblStripeConnectAccountUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "5ecca51a-7fd3-4143-a97c-c682c9d404b1", new DateTime(2024, 4, 23, 23, 37, 11, 442, DateTimeKind.Local).AddTicks(7649), "AQAAAAEAACcQAAAAEO5xDsavyBNkCACP4S+56kfDTBr+FGNTzw6I2H3CJNTAt1gG4ep+jGXsBaq3HGRVww==", "631c5286-9bad-488d-be57-6fc4a7ba9e05", new DateTime(2024, 4, 23, 23, 37, 11, 442, DateTimeKind.Local).AddTicks(7661) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Payouts_Enabled",
                table: "TblStripeConnectAccountUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "1bd7fe93-3e76-4342-b3d0-902c08b953f9", new DateTime(2024, 4, 23, 21, 13, 54, 345, DateTimeKind.Local).AddTicks(9606), "AQAAAAEAACcQAAAAEInq9Grho2asjI9bjamIY9qdXMl9ELt6GK+o5zk9araU6of0MRmPnR8+omPjX5VHWQ==", "dbd67089-777c-43a8-9817-92ba18ef20c9", new DateTime(2024, 4, 23, 21, 13, 54, 345, DateTimeKind.Local).AddTicks(9616) });
        }
    }
}
