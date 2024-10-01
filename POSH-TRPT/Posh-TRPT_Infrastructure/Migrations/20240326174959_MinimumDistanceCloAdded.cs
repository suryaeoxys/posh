using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class MinimumDistanceCloAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MinimumDistance",
                table: "TblBookingDetail",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "b13c5caa-7132-46e5-b537-e3355ff15e22", new DateTime(2024, 3, 26, 23, 19, 58, 890, DateTimeKind.Local).AddTicks(4081), "AQAAAAEAACcQAAAAEAMYa9nBoBHjyAVPqUBLhdlGftyge9KhlibIt5BBoDnTbcAe+eiDmpyGmHVmBt4Lqw==", "87bc3e93-a3d3-47ed-be59-590d9e7f7318", new DateTime(2024, 3, 26, 23, 19, 58, 890, DateTimeKind.Local).AddTicks(4089) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinimumDistance",
                table: "TblBookingDetail");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "b5f263fa-23a3-4780-baea-cb1f14fe6196", new DateTime(2024, 3, 26, 22, 57, 5, 279, DateTimeKind.Local).AddTicks(6310), "AQAAAAEAACcQAAAAEGgqDNBC6rcapOIIKOlEjATpP0DGu52c6Fyzgy82yXsuxQUk15awWAqDshXEBKdbbw==", "e6d2b75f-665e-4db6-ab31-59eaf8d0f2cc", new DateTime(2024, 3, 26, 22, 57, 5, 279, DateTimeKind.Local).AddTicks(6324) });
        }
    }
}
