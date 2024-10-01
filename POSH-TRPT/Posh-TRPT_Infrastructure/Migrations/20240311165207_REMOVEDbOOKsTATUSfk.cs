using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class REMOVEDbOOKsTATUSfk : Migration
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
                values: new object[] { "7b961f60-d56f-4450-bdbd-4a15939bd2e9", new DateTime(2024, 3, 11, 22, 22, 7, 426, DateTimeKind.Local).AddTicks(5034), "AQAAAAEAACcQAAAAEPRs05dh53WGd4K+LYlnuPr5uRkeZXl1dkZ5hcwbmfVT3lK7nHnRYJYx5uhLOif4/g==", "c61d42f9-9c68-42c3-a40e-a0b104b1ae59", new DateTime(2024, 3, 11, 22, 22, 7, 426, DateTimeKind.Local).AddTicks(5043) });
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
                values: new object[] { "2a863de6-782b-4ff1-8d90-7ea0c029c286", new DateTime(2024, 3, 11, 16, 26, 8, 606, DateTimeKind.Local).AddTicks(9971), "AQAAAAEAACcQAAAAEAvmYN9H8so4DFTEpuCy/Xti/GoByvWBm5KScS3eiRhByWT/RIlnPrefs4LF/fm7Qw==", "10c68245-b68c-4c06-94c6-c2620b45f295", new DateTime(2024, 3, 11, 16, 26, 8, 606, DateTimeKind.Local).AddTicks(9980) });

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
