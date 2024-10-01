using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedRiderDeviceId : Migration
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
                values: new object[] { "6637a6cd-9ba1-463f-93f5-356f67f9b65c", new DateTime(2024, 3, 26, 23, 33, 36, 508, DateTimeKind.Local).AddTicks(4439), "AQAAAAEAACcQAAAAEF+l+L67Z9ilJZmSZMSlfCjOHYmYMO79ckTDkWFWI88s2KyCnb10gJ/8WAYC/7KN0w==", "084335a6-78da-45f5-a249-3a6598db6fa7", new DateTime(2024, 3, 26, 23, 33, 36, 508, DateTimeKind.Local).AddTicks(4447) });
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
                values: new object[] { "b13c5caa-7132-46e5-b537-e3355ff15e22", new DateTime(2024, 3, 26, 23, 19, 58, 890, DateTimeKind.Local).AddTicks(4081), "AQAAAAEAACcQAAAAEAMYa9nBoBHjyAVPqUBLhdlGftyge9KhlibIt5BBoDnTbcAe+eiDmpyGmHVmBt4Lqw==", "87bc3e93-a3d3-47ed-be59-590d9e7f7318", new DateTime(2024, 3, 26, 23, 19, 58, 890, DateTimeKind.Local).AddTicks(4089) });
        }
    }
}
