using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedBookingStatusId4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[] { "b7dfb664-67a2-44b4-8fdd-862bc6b753a3", new DateTime(2024, 3, 12, 20, 46, 35, 299, DateTimeKind.Local).AddTicks(2401), "AQAAAAEAACcQAAAAEASvFBTpcEQ7HQ2ewVFEjn7eMpbk3Ba0M9KkZ75lm4ljXyLO2flX6+0cieU3IEMdjA==", "6f028db2-8e6b-4618-85e3-3afb2d601bba", new DateTime(2024, 3, 12, 20, 46, 35, 299, DateTimeKind.Local).AddTicks(2412) });
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
                values: new object[] { "9119326f-6985-484d-8dbc-b36029f2a191", new DateTime(2024, 3, 12, 20, 46, 4, 765, DateTimeKind.Local).AddTicks(168), "AQAAAAEAACcQAAAAEF6e1SwLcioYvzYkLaBvknezFbqvEW+xe0TN9n6CDyEGlcd3BBe9oHw+fJoRG2SAvw==", "43651497-760e-4e42-801e-69f24ad6fc54", new DateTime(2024, 3, 12, 20, 46, 4, 765, DateTimeKind.Local).AddTicks(177) });
        }
    }
}
