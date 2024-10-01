using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class BookingStatusFKRem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[] { "95ae5ae1-828c-4997-b277-90a6090bbe0a", new DateTime(2024, 3, 12, 20, 17, 25, 565, DateTimeKind.Local).AddTicks(2495), "AQAAAAEAACcQAAAAEKpDLniTH9YhsxDSHwergspwejiTU/QZfx9yNwMISHi3XNfrZT8U35arLylHatG9BQ==", "1103dff7-fa32-4dc6-a9a3-8570035a6b3f", new DateTime(2024, 3, 12, 20, 17, 25, 565, DateTimeKind.Local).AddTicks(2503) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
