using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedBookingStatusId3 : Migration
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
                values: new object[] { "9119326f-6985-484d-8dbc-b36029f2a191", new DateTime(2024, 3, 12, 20, 46, 4, 765, DateTimeKind.Local).AddTicks(168), "AQAAAAEAACcQAAAAEF6e1SwLcioYvzYkLaBvknezFbqvEW+xe0TN9n6CDyEGlcd3BBe9oHw+fJoRG2SAvw==", "43651497-760e-4e42-801e-69f24ad6fc54", new DateTime(2024, 3, 12, 20, 46, 4, 765, DateTimeKind.Local).AddTicks(177) });
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
                values: new object[] { "4e82cebb-b8f1-4df6-9d54-6e103e153bb1", new DateTime(2024, 3, 12, 20, 45, 6, 4, DateTimeKind.Local).AddTicks(9638), "AQAAAAEAACcQAAAAEGkmWWkRASY4R6cOCaRJcabFrQY76g+hYvuZnBxZiJgPhhMWTwUIQwRyZqXKPC1aWQ==", "2bc1ec53-d082-4a1c-92d1-532406887364", new DateTime(2024, 3, 12, 20, 45, 6, 4, DateTimeKind.Local).AddTicks(9646) });
        }
    }
}
