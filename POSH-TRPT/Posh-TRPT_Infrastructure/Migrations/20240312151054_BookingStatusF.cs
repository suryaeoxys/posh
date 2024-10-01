using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class BookingStatusF : Migration
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
                values: new object[] { "42ff01df-5eb6-4d8e-9e5b-eee735c6cd99", new DateTime(2024, 3, 12, 20, 40, 54, 530, DateTimeKind.Local).AddTicks(3933), "AQAAAAEAACcQAAAAECegTSnWVHRVP7MCVpp+fNE7U/asdrzB+MfNS/0yXHMevCQBw1++C1MoOizPqZUi5A==", "0a49a323-b03d-4fab-ae8a-1a734709ce30", new DateTime(2024, 3, 12, 20, 40, 54, 530, DateTimeKind.Local).AddTicks(3948) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BookingStatusId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("fb09bef3-b1f2-4e21-a25d-a2d769315778"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "fd6ff618-d3d4-428f-9d6e-d724448f83c6", new DateTime(2024, 3, 12, 20, 31, 53, 504, DateTimeKind.Local).AddTicks(706), "AQAAAAEAACcQAAAAEBL02Ehya2E+H8KZ0JmXQLgNGGKIuJOjbfBmYE5R301HFOEwYPBcaLmfXQLetzPVPQ==", "6e86c271-83a2-4fa1-b36a-a2e50ff11de6", new DateTime(2024, 3, 12, 20, 31, 53, 504, DateTimeKind.Local).AddTicks(713) });

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
