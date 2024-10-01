using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class addedPayouts_Enabled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Payouts_Enabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "a4241d04-bb83-47c0-9093-63ab13818353", new DateTime(2024, 4, 24, 18, 52, 0, 281, DateTimeKind.Local).AddTicks(5867), "AQAAAAEAACcQAAAAEF59/rHqWvE3YU2PmxF95JOsshR8ADkZFG0bEHg9XbHjObgYkVhSgQdDTOalt8y5bQ==", "5952493b-0077-460e-9bf8-b02fc124f7b8", new DateTime(2024, 4, 24, 18, 52, 0, 281, DateTimeKind.Local).AddTicks(5877) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Payouts_Enabled",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "802ef88c-0057-444f-a956-32a9f3288fd2", new DateTime(2024, 4, 24, 15, 57, 57, 212, DateTimeKind.Local).AddTicks(4902), "AQAAAAEAACcQAAAAEIaNSWSu32aYLPkwBuRPLADPdtkm6gtaqXLP5ZUOpQ7IHuAKCVb9oATj3aUI8HFcdQ==", "3b87ae09-1315-42c5-8887-230aab5a7da9", new DateTime(2024, 4, 24, 15, 57, 57, 212, DateTimeKind.Local).AddTicks(4913) });
        }
    }
}
