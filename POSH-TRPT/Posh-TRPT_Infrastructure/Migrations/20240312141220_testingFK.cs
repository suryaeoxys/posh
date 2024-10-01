using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class testingFK : Migration
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
                values: new object[] { "87af313e-6fbe-4669-9b7d-ffc8ee973adf", new DateTime(2024, 3, 12, 19, 42, 20, 50, DateTimeKind.Local).AddTicks(2089), "AQAAAAEAACcQAAAAEGzyMVHIDFxP+7IOpsYdH1mpnP4emIoibsrR6Pme+TemIUqiT+w/C0P81lu/NV7h1A==", "f2989cb6-2d94-468f-aff9-cdccbb0a8512", new DateTime(2024, 3, 12, 19, 42, 20, 50, DateTimeKind.Local).AddTicks(2097) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BookingStatusId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "20aba648-75ce-44a9-9f04-e814a3ad56b3", new DateTime(2024, 3, 12, 16, 2, 7, 637, DateTimeKind.Local).AddTicks(3763), "AQAAAAEAACcQAAAAEJgtVf/ASstCusbCgtEvInyaZgtyO1zqueqrDwHOnhn0KC06ws1DbAJ54mVHFO53fw==", "b880d316-d8cd-400f-b66b-f0e6e1d245b3", new DateTime(2024, 3, 12, 16, 2, 7, 637, DateTimeKind.Local).AddTicks(3776) });
        }
    }
}
