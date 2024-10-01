using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class InitialDBLocal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "73bc3ab2-0e42-471c-99c1-72986b8bc12e", new DateTime(2024, 3, 18, 20, 18, 3, 601, DateTimeKind.Local).AddTicks(5148), "AQAAAAEAACcQAAAAEPEP3gSaivqMpvss9ivK9XbrV56B/xFwfRZpYW9AJAoCFEwWaePMHF57LErStBEk+w==", "cff701eb-2f45-4871-ac43-9f51d5767a5c", new DateTime(2024, 3, 18, 20, 18, 3, 601, DateTimeKind.Local).AddTicks(5155) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "e300519d-c8fa-46f8-b53f-1282d1177e4c", new DateTime(2024, 3, 12, 20, 47, 36, 859, DateTimeKind.Local).AddTicks(7567), "AQAAAAEAACcQAAAAECz9atry2hbAmAhjDnnV1jkRDmyQ/3gUWmqIxUp1Xw0nxk6CrSa45Pi8y2XOvTgkCw==", "003c1847-f258-4ce2-9d1d-e094a6b41a7b", new DateTime(2024, 3, 12, 20, 47, 36, 859, DateTimeKind.Local).AddTicks(7577) });
        }
    }
}
