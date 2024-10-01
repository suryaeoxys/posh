using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedStatusTypoe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StatusType",
                table: "TblBookingDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "d018d2d9-6bc0-4f0f-924a-f51818a5d814", new DateTime(2024, 4, 5, 18, 57, 29, 786, DateTimeKind.Local).AddTicks(7897), "AQAAAAEAACcQAAAAENJGFwMDxYpF//HwYF1c5R6V31p9NnRttwd4jT3sbx7+rLG+2KsvxNBovNqn9WPB6g==", "e13378b0-2188-4368-83c6-af6f7dae8467", new DateTime(2024, 4, 5, 18, 57, 29, 786, DateTimeKind.Local).AddTicks(7905) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusType",
                table: "TblBookingDetail");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "39b18241-d081-485b-9342-9d3ad4cdec95", new DateTime(2024, 4, 1, 22, 29, 2, 609, DateTimeKind.Local).AddTicks(3213), "AQAAAAEAACcQAAAAEAfhqt+q+oUMH3MtGzTiZoN/XXV5SVP6D1xLFmfHepvASHgSBZHPNnyq8ktqWCCBNg==", "af2ca18b-3c10-419d-8408-e193a5d769a8", new DateTime(2024, 4, 1, 22, 29, 2, 609, DateTimeKind.Local).AddTicks(3226) });
        }
    }
}
