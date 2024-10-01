using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class addedDriverLongDriverLatColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DriverLat",
                table: "TblBookingDetail",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DriverLong",
                table: "TblBookingDetail",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "e48f1863-ae73-4a0d-8f6e-6e9549d19670", new DateTime(2024, 3, 28, 20, 48, 8, 122, DateTimeKind.Local).AddTicks(4424), "AQAAAAEAACcQAAAAED+6Za9pP8i2Ws4BgglJz8h9GKz6A/53iagKiFa6sBrWviDibsaMrj/JfyWjnPyYYw==", "ad7b59d7-ed02-44db-89f4-88ace64ff5a5", new DateTime(2024, 3, 28, 20, 48, 8, 122, DateTimeKind.Local).AddTicks(4433) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverLat",
                table: "TblBookingDetail");

            migrationBuilder.DropColumn(
                name: "DriverLong",
                table: "TblBookingDetail");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "060f86a9-05cc-43b7-b3f0-0098a6cb147a", new DateTime(2024, 3, 27, 22, 23, 37, 108, DateTimeKind.Local).AddTicks(1683), "AQAAAAEAACcQAAAAEARWQQXGUV3MHtxPtuE8rcsGWPfPcRwWcNXPw41tyl/IzxHJfBdFJDz2SioK56o0UQ==", "0aa13712-5df8-4eab-99bd-258cd2b7dd6d", new DateTime(2024, 3, 27, 22, 23, 37, 108, DateTimeKind.Local).AddTicks(1692) });
        }
    }
}
