using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class InitialDBLocal1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "6bca91a5-e3e9-4cf0-a919-4e34d54348f0", new DateTime(2024, 3, 18, 20, 42, 3, 50, DateTimeKind.Local).AddTicks(477), "AQAAAAEAACcQAAAAENPOHRWj0SlTxN12ocSc2viib3i8phfL7suD8IAJ+YmSOuQqHHgH6Gjoh9z/dAgmBA==", "0a0600af-3247-4191-a8fb-3b36380980c6", new DateTime(2024, 3, 18, 20, 42, 3, 50, DateTimeKind.Local).AddTicks(484) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "73bc3ab2-0e42-471c-99c1-72986b8bc12e", new DateTime(2024, 3, 18, 20, 18, 3, 601, DateTimeKind.Local).AddTicks(5148), "AQAAAAEAACcQAAAAEPEP3gSaivqMpvss9ivK9XbrV56B/xFwfRZpYW9AJAoCFEwWaePMHF57LErStBEk+w==", "cff701eb-2f45-4871-ac43-9f51d5767a5c", new DateTime(2024, 3, 18, 20, 18, 3, 601, DateTimeKind.Local).AddTicks(5155) });
        }
    }
}
