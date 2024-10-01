using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class removedFKBookingStatusId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_TblBookingStatus_BookingStatusId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BookingStatusId",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "801a75d8-756a-496e-9a83-5af12a80b764", new DateTime(2024, 3, 12, 16, 0, 36, 440, DateTimeKind.Local).AddTicks(9223), "AQAAAAEAACcQAAAAEP1N1SkRs2GmJlJp7Bx/7/pLPt6I1gl06HYBvzmFqDN+iiA8keVcq00jYxU5sPWhCQ==", "28c41cbb-f33a-416f-96d0-05598daf59b9", new DateTime(2024, 3, 12, 16, 0, 36, 440, DateTimeKind.Local).AddTicks(9232) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
