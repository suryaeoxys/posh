using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class testingFK1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BookingStatusId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "f0544d52-fa92-4316-8dc0-683dcda4ac11", new DateTime(2024, 3, 12, 19, 54, 55, 305, DateTimeKind.Local).AddTicks(2016), "AQAAAAEAACcQAAAAECitWjY6QW4TErYgpSKhzimWnYSkpjZJG04Sr84UZFORiVW+9sXJMvg3WOQWepfhdg==", "1844dc07-fe09-479a-9f2b-66e51d394883", new DateTime(2024, 3, 12, 19, 54, 55, 305, DateTimeKind.Local).AddTicks(2026) });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BookingStatusId",
                table: "AspNetUsers",
                column: "BookingStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_TblBookingStatus_BookingStatusId",
                table: "AspNetUsers",
                column: "BookingStatusId",
                principalTable: "TblBookingStatus",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_TblBookingStatus_BookingStatusId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BookingStatusId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BookingStatusId",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "87af313e-6fbe-4669-9b7d-ffc8ee973adf", new DateTime(2024, 3, 12, 19, 42, 20, 50, DateTimeKind.Local).AddTicks(2089), "AQAAAAEAACcQAAAAEGzyMVHIDFxP+7IOpsYdH1mpnP4emIoibsrR6Pme+TemIUqiT+w/C0P81lu/NV7h1A==", "f2989cb6-2d94-468f-aff9-cdccbb0a8512", new DateTime(2024, 3, 12, 19, 42, 20, 50, DateTimeKind.Local).AddTicks(2097) });
        }
    }
}
