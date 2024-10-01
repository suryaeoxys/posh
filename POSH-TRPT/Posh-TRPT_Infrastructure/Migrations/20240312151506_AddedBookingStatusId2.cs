using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedBookingStatusId2 : Migration
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
                values: new object[] { "4e82cebb-b8f1-4df6-9d54-6e103e153bb1", new DateTime(2024, 3, 12, 20, 45, 6, 4, DateTimeKind.Local).AddTicks(9638), "AQAAAAEAACcQAAAAEGkmWWkRASY4R6cOCaRJcabFrQY76g+hYvuZnBxZiJgPhhMWTwUIQwRyZqXKPC1aWQ==", "2bc1ec53-d082-4a1c-92d1-532406887364", new DateTime(2024, 3, 12, 20, 45, 6, 4, DateTimeKind.Local).AddTicks(9646) });
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
                values: new object[] { "64cef93d-9e6e-4fb0-b51c-3d9a49bde4a6", new DateTime(2024, 3, 12, 20, 43, 57, 873, DateTimeKind.Local).AddTicks(5229), "AQAAAAEAACcQAAAAEMUNqDB87evy3fKU/80JVzA4037+OK2KY6WfSwrkUav4VL7Rv8Tmh16YFkBKQiVtsw==", "12a350e9-a6b1-46f9-aaad-c25d7cae9abb", new DateTime(2024, 3, 12, 20, 43, 57, 873, DateTimeKind.Local).AddTicks(5237) });
        }
    }
}
