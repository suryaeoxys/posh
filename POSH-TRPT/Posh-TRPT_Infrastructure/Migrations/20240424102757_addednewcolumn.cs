using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class addednewcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StripeConnectedAccountId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "802ef88c-0057-444f-a956-32a9f3288fd2", new DateTime(2024, 4, 24, 15, 57, 57, 212, DateTimeKind.Local).AddTicks(4902), "AQAAAAEAACcQAAAAEIaNSWSu32aYLPkwBuRPLADPdtkm6gtaqXLP5ZUOpQ7IHuAKCVb9oATj3aUI8HFcdQ==", "3b87ae09-1315-42c5-8887-230aab5a7da9", new DateTime(2024, 4, 24, 15, 57, 57, 212, DateTimeKind.Local).AddTicks(4913) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeConnectedAccountId",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "5ecca51a-7fd3-4143-a97c-c682c9d404b1", new DateTime(2024, 4, 23, 23, 37, 11, 442, DateTimeKind.Local).AddTicks(7649), "AQAAAAEAACcQAAAAEO5xDsavyBNkCACP4S+56kfDTBr+FGNTzw6I2H3CJNTAt1gG4ep+jGXsBaq3HGRVww==", "631c5286-9bad-488d-be57-6fc4a7ba9e05", new DateTime(2024, 4, 23, 23, 37, 11, 442, DateTimeKind.Local).AddTicks(7661) });
        }
    }
}
