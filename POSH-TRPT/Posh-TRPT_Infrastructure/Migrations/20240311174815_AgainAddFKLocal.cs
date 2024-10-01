using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AgainAddFKLocal : Migration
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
                values: new object[] { "64dac958-dc8c-4e1d-818b-129cd44262b7", new DateTime(2024, 3, 11, 23, 18, 14, 917, DateTimeKind.Local).AddTicks(9296), "AQAAAAEAACcQAAAAEIGMdQHVJffNbrlZSXz35+lDvUE8TrCVgsSAMSvTGV6bm9G+LKj5pEB3CZlbvJ7MsQ==", "6c870637-9a1b-420e-8376-0fdaad633f49", new DateTime(2024, 3, 11, 23, 18, 14, 917, DateTimeKind.Local).AddTicks(9305) });

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
                values: new object[] { "2a7a732c-e682-4ff0-8255-4b3b094e780b", new DateTime(2024, 3, 11, 23, 14, 43, 362, DateTimeKind.Local).AddTicks(2674), "AQAAAAEAACcQAAAAEJo2/4Q42dVuDkbqvGj7TBM53FMeguo/nhKeGObbh0NpxkX7IjvyDwuCR6+jPgcbVQ==", "17c0a5c0-681a-4361-ab92-d004f7eb92ba", new DateTime(2024, 3, 11, 23, 14, 43, 362, DateTimeKind.Local).AddTicks(2686) });
        }
    }
}
