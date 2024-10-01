using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class NewColumnStripeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StripeCustomerId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "0ffe30ea-48f0-4cb4-b801-75b8b859d88c", new DateTime(2024, 4, 22, 19, 45, 55, 297, DateTimeKind.Local).AddTicks(1671), "AQAAAAEAACcQAAAAEOxUJIlbqZRyHKWkLgzlZ5zPI/Yt6Pmm9uVr2W7h1dZaalNdeBnmRsXYsSpb4nEs4Q==", "011d7373-b9ae-412d-a4b6-5d7bb41d2b75", new DateTime(2024, 4, 22, 19, 45, 55, 297, DateTimeKind.Local).AddTicks(1681) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeCustomerId",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "0f8c5b6b-22fc-4bc7-8e57-bff12cc2bd61", new DateTime(2024, 4, 22, 19, 11, 8, 70, DateTimeKind.Local).AddTicks(1099), "AQAAAAEAACcQAAAAEHi2XM09WecTc8dYGitBEUzXraSXTY0I3qezd7o0WsypGMeuRios7f4tf9rc7zxOvg==", "1f091f0c-9e02-46e4-b13b-73c8e5823a4c", new DateTime(2024, 4, 22, 19, 11, 8, 70, DateTimeKind.Local).AddTicks(1108) });
        }
    }
}
