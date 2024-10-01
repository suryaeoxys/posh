using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class addedPayouts_Enabled1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "c5287c8a-aed7-4d0d-bd6f-f13026419d86", new DateTime(2024, 4, 25, 2, 0, 34, 556, DateTimeKind.Local).AddTicks(6839), "AQAAAAEAACcQAAAAECoLwcWV82///3eoi9CrA0gJrDb3lH2X3GvCC4DACZgyGZ2BXoglFzawCHPYD5us6Q==", "ac5963cb-3d63-4656-828b-7c2f12e16d37", new DateTime(2024, 4, 25, 2, 0, 34, 556, DateTimeKind.Local).AddTicks(6848) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "a4241d04-bb83-47c0-9093-63ab13818353", new DateTime(2024, 4, 24, 18, 52, 0, 281, DateTimeKind.Local).AddTicks(5867), "AQAAAAEAACcQAAAAEF59/rHqWvE3YU2PmxF95JOsshR8ADkZFG0bEHg9XbHjObgYkVhSgQdDTOalt8y5bQ==", "5952493b-0077-460e-9bf8-b02fc124f7b8", new DateTime(2024, 4, 24, 18, 52, 0, 281, DateTimeKind.Local).AddTicks(5877) });
        }
    }
}
