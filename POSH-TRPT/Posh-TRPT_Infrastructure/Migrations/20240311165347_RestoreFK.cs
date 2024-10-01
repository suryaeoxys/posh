using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class RestoreFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BookingStatusId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("FB09BEF3-B1F2-4E21-A25D-A2D769315778"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "8fbe080a-6703-4097-9168-61592479323d", new DateTime(2024, 3, 11, 22, 23, 46, 730, DateTimeKind.Local).AddTicks(9739), "AQAAAAEAACcQAAAAEHiHZdmLFGCV/25S1MG2Z52x/7q9/r6QtdD96ooXxLg73lByPccfO7PhPJqtdOubqA==", "625c360f-93a5-40bd-9fc9-a02c1b50f72f", new DateTime(2024, 3, 11, 22, 23, 46, 730, DateTimeKind.Local).AddTicks(9746) });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BookingStatusId",
                table: "AspNetUsers",
                column: "BookingStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_TblBookingStatus_BookingStatusId",
                table: "AspNetUsers",
                column: "BookingStatusId",
                principalTable: "TblBookingStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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
                values: new object[] { "7b961f60-d56f-4450-bdbd-4a15939bd2e9", new DateTime(2024, 3, 11, 22, 22, 7, 426, DateTimeKind.Local).AddTicks(5034), "AQAAAAEAACcQAAAAEPRs05dh53WGd4K+LYlnuPr5uRkeZXl1dkZ5hcwbmfVT3lK7nHnRYJYx5uhLOif4/g==", "c61d42f9-9c68-42c3-a40e-a0b104b1ae59", new DateTime(2024, 3, 11, 22, 22, 7, 426, DateTimeKind.Local).AddTicks(5043) });
        }
    }
}
