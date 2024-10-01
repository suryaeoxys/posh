using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedBookingStatusId1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingStatusId",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "64cef93d-9e6e-4fb0-b51c-3d9a49bde4a6", new DateTime(2024, 3, 12, 20, 43, 57, 873, DateTimeKind.Local).AddTicks(5229), "AQAAAAEAACcQAAAAEMUNqDB87evy3fKU/80JVzA4037+OK2KY6WfSwrkUav4VL7Rv8Tmh16YFkBKQiVtsw==", "12a350e9-a6b1-46f9-aaad-c25d7cae9abb", new DateTime(2024, 3, 12, 20, 43, 57, 873, DateTimeKind.Local).AddTicks(5237) });
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
                values: new object[] { "ac30d62f-8040-4bab-a3d3-565d8625df12", new DateTime(2024, 3, 12, 20, 42, 15, 829, DateTimeKind.Local).AddTicks(3288), "AQAAAAEAACcQAAAAEBBuVfn20pM6xuaois+HTTvWDfXy4iqEDk4KLBetRsrcw6Zim4XpBpP8P6rKphQStg==", "d7692436-5d68-4f35-80a7-c2e17e5415e2", new DateTime(2024, 3, 12, 20, 42, 15, 829, DateTimeKind.Local).AddTicks(3297) });
        }
    }
}
