using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class MadeNullableDocsColumnDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "b76d8566-aa82-45ec-a31a-b055c8dad272", new DateTime(2024, 2, 8, 17, 8, 37, 905, DateTimeKind.Local).AddTicks(2738), "AQAAAAEAACcQAAAAEFOaPPWmOrICflvxnBRXkZwXhqe4dmv9w28xAyV6EhJfc3HmZzy5Lo40BkzUU1hy4w==", "aaac1b69-1168-454c-9516-b6ef9b5f1ed2", new DateTime(2024, 2, 8, 17, 8, 37, 905, DateTimeKind.Local).AddTicks(2746) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "248c77d3-d0da-467b-b144-dec0eccbc5f4", new DateTime(2024, 2, 7, 22, 23, 13, 20, DateTimeKind.Local).AddTicks(9369), "AQAAAAEAACcQAAAAEP7L3e7amzTaCxRQpdcmN1RYdgw42gNwM+8vdHPtyMjMea686eLh3f8upO5a+l4VzA==", "2e8938f0-fac7-4c3e-96ad-949cad5a485c", new DateTime(2024, 2, 7, 22, 23, 13, 20, DateTimeKind.Local).AddTicks(9389) });
        }
    }
}
