using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class removedColumnDeviceId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinimumDistance",
                table: "TblBookingDetail");

            migrationBuilder.DropColumn(
                name: "RiderDeviceId",
                table: "TblBookingDetail");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "f182c32e-729a-4503-a66e-ad9e7ae96804", new DateTime(2024, 3, 27, 19, 29, 26, 455, DateTimeKind.Local).AddTicks(2819), "AQAAAAEAACcQAAAAEGZzvxeYhfOTTE0LKRsYcsLJn/lqemK5q3jdrsfqKjizNcgLrbsV8HuG9MNYFIq8HQ==", "011ef05a-f90a-4c53-ab8f-1337da188066", new DateTime(2024, 3, 27, 19, 29, 26, 455, DateTimeKind.Local).AddTicks(2828) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MinimumDistance",
                table: "TblBookingDetail",
                type: "float",
                nullable: true);

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
                values: new object[] { "6637a6cd-9ba1-463f-93f5-356f67f9b65c", new DateTime(2024, 3, 26, 23, 33, 36, 508, DateTimeKind.Local).AddTicks(4439), "AQAAAAEAACcQAAAAEF+l+L67Z9ilJZmSZMSlfCjOHYmYMO79ckTDkWFWI88s2KyCnb10gJ/8WAYC/7KN0w==", "084335a6-78da-45f5-a249-3a6598db6fa7", new DateTime(2024, 3, 26, 23, 33, 36, 508, DateTimeKind.Local).AddTicks(4447) });
        }
    }
}
