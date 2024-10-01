using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedBookingStatusId5 : Migration
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
                values: new object[] { "2c591d79-3ef4-43ee-bd77-436a5c044d1c", new DateTime(2024, 3, 12, 20, 47, 11, 294, DateTimeKind.Local).AddTicks(9857), "AQAAAAEAACcQAAAAED7dIUH3CkojdiINCUANQmErZ2T43e7dJh2KeKjaUIqpW8haTYF2qpGb72/nzxIdwA==", "dca48cf8-f2c4-4797-890e-5975115fc160", new DateTime(2024, 3, 12, 20, 47, 11, 294, DateTimeKind.Local).AddTicks(9868) });
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
                values: new object[] { "b7dfb664-67a2-44b4-8fdd-862bc6b753a3", new DateTime(2024, 3, 12, 20, 46, 35, 299, DateTimeKind.Local).AddTicks(2401), "AQAAAAEAACcQAAAAEASvFBTpcEQ7HQ2ewVFEjn7eMpbk3Ba0M9KkZ75lm4ljXyLO2flX6+0cieU3IEMdjA==", "6f028db2-8e6b-4618-85e3-3afb2d601bba", new DateTime(2024, 3, 12, 20, 46, 35, 299, DateTimeKind.Local).AddTicks(2412) });
        }
    }
}
