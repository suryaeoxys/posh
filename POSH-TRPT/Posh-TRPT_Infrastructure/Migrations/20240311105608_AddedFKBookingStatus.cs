using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedFKBookingStatus : Migration
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
                values: new object[] { "f692dfe9-ccb8-4d31-a842-eb782c994fa5", new DateTime(2024, 3, 11, 15, 19, 25, 275, DateTimeKind.Local).AddTicks(863), "AQAAAAEAACcQAAAAEBo3rCaL2lFWASCWxfw6ofhIdtOYuGFw2JEI4kWbWwzEDCBxix17fd5+eHwE9410AA==", "51d8cfc5-4762-49cc-9c6c-7ed4f8da753d", new DateTime(2024, 3, 11, 15, 19, 25, 275, DateTimeKind.Local).AddTicks(876) });
        }
    }
}
