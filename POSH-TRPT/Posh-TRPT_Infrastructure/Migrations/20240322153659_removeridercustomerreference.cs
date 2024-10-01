using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class removeridercustomerreference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblBookingDetail_AspNetUsers_DriverUserId",
                table: "TblBookingDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_TblBookingDetail_AspNetUsers_RiderUserId",
                table: "TblBookingDetail");

            migrationBuilder.DropIndex(
                name: "IX_TblBookingDetail_DriverUserId",
                table: "TblBookingDetail");

            migrationBuilder.DropIndex(
                name: "IX_TblBookingDetail_RiderUserId",
                table: "TblBookingDetail");

            migrationBuilder.DropColumn(
                name: "DriverUserId",
                table: "TblBookingDetail");

            migrationBuilder.DropColumn(
                name: "RiderUserId",
                table: "TblBookingDetail");

            migrationBuilder.AlterColumn<string>(
                name: "RiderId",
                table: "TblBookingDetail",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DriverId",
                table: "TblBookingDetail",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "5590a96c-93fd-4b31-a501-5d2116b808f5", new DateTime(2024, 3, 22, 21, 6, 58, 870, DateTimeKind.Local).AddTicks(7428), "AQAAAAEAACcQAAAAEIeZCjrjJzDQqDf1ljaYzJXXTSIvY/SKGAV4LiK0XoCCH4sUfZeC20cyMZpAG7teLg==", "9a9e1bf7-f2a5-4b35-9b13-396673b8a8dc", new DateTime(2024, 3, 22, 21, 6, 58, 870, DateTimeKind.Local).AddTicks(7436) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RiderId",
                table: "TblBookingDetail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DriverId",
                table: "TblBookingDetail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriverUserId",
                table: "TblBookingDetail",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RiderUserId",
                table: "TblBookingDetail",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "38528130-3979-4166-980a-6d7056af2531", new DateTime(2024, 3, 22, 20, 56, 2, 189, DateTimeKind.Local).AddTicks(1097), "AQAAAAEAACcQAAAAEHFigoL6nticCYzP/7xz/qqEIJ8i5MqD5/g11xWfxj2Lu/XgHOUIeOL5g1h+aUW3Mw==", "465e8d54-84a9-4cef-bfbd-41df192d9947", new DateTime(2024, 3, 22, 20, 56, 2, 189, DateTimeKind.Local).AddTicks(1111) });

            migrationBuilder.CreateIndex(
                name: "IX_TblBookingDetail_DriverUserId",
                table: "TblBookingDetail",
                column: "DriverUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TblBookingDetail_RiderUserId",
                table: "TblBookingDetail",
                column: "RiderUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TblBookingDetail_AspNetUsers_DriverUserId",
                table: "TblBookingDetail",
                column: "DriverUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TblBookingDetail_AspNetUsers_RiderUserId",
                table: "TblBookingDetail",
                column: "RiderUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
