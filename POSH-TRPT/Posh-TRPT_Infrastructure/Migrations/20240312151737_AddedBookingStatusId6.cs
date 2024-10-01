using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedBookingStatusId6 : Migration
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
                values: new object[] { "e300519d-c8fa-46f8-b53f-1282d1177e4c", new DateTime(2024, 3, 12, 20, 47, 36, 859, DateTimeKind.Local).AddTicks(7567), "AQAAAAEAACcQAAAAECz9atry2hbAmAhjDnnV1jkRDmyQ/3gUWmqIxUp1Xw0nxk6CrSa45Pi8y2XOvTgkCw==", "003c1847-f258-4ce2-9d1d-e094a6b41a7b", new DateTime(2024, 3, 12, 20, 47, 36, 859, DateTimeKind.Local).AddTicks(7577) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingStatusId",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "2c591d79-3ef4-43ee-bd77-436a5c044d1c", new DateTime(2024, 3, 12, 20, 47, 11, 294, DateTimeKind.Local).AddTicks(9857), "AQAAAAEAACcQAAAAED7dIUH3CkojdiINCUANQmErZ2T43e7dJh2KeKjaUIqpW8haTYF2qpGb72/nzxIdwA==", "dca48cf8-f2c4-4797-890e-5975115fc160", new DateTime(2024, 3, 12, 20, 47, 11, 294, DateTimeKind.Local).AddTicks(9868) });
        }
    }
}
