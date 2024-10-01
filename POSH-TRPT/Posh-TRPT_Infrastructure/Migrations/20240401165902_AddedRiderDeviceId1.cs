using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedRiderDeviceId1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RiderDeviceId",
                table: "TblBookingDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "39b18241-d081-485b-9342-9d3ad4cdec95", new DateTime(2024, 4, 1, 22, 29, 2, 609, DateTimeKind.Local).AddTicks(3213), "AQAAAAEAACcQAAAAEAfhqt+q+oUMH3MtGzTiZoN/XXV5SVP6D1xLFmfHepvASHgSBZHPNnyq8ktqWCCBNg==", "af2ca18b-3c10-419d-8408-e193a5d769a8", new DateTime(2024, 4, 1, 22, 29, 2, 609, DateTimeKind.Local).AddTicks(3226) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RiderDeviceId",
                table: "TblBookingDetail");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "17cf80ac-0531-4bc3-9482-983571580d5b", new DateTime(2024, 4, 1, 22, 16, 3, 462, DateTimeKind.Local).AddTicks(4090), "AQAAAAEAACcQAAAAEKQoMdA8YJDPuZR5QUUdbO/AU0LW61UNKxgsOGRrUsnSWf+NMoB4SpxZ9WRi2Og1YA==", "8729038d-2f22-493a-a8ee-cc785a68373c", new DateTime(2024, 4, 1, 22, 16, 3, 462, DateTimeKind.Local).AddTicks(4100) });
        }
    }
}
