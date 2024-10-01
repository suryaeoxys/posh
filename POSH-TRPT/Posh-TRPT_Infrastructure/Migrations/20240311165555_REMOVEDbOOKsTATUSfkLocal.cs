using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class REMOVEDbOOKsTATUSfkLocal : Migration
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
                values: new object[] { "f24d3ba0-0980-4cc8-b402-2d80bc9511c3", new DateTime(2024, 3, 11, 22, 25, 55, 390, DateTimeKind.Local).AddTicks(1723), "AQAAAAEAACcQAAAAEEqQfFw2jZXzLDECFNFWPDI67tQdbOp9/wKfiuvVnLQPXVC+clOJSrYQdJdtMpVWqg==", "7c97dcad-d854-4d91-8609-4e57f10555c2", new DateTime(2024, 3, 11, 22, 25, 55, 390, DateTimeKind.Local).AddTicks(1731) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BookingStatusId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
    }
}
