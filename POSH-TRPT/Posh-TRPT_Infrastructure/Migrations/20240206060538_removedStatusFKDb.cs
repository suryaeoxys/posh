using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class removedStatusFKDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "5eff80ec-1d82-4c07-ae43-274450246fe7", new DateTime(2024, 2, 6, 11, 35, 37, 467, DateTimeKind.Local).AddTicks(3108), "AQAAAAEAACcQAAAAECQw+vr7mxiWRHFh3Fgi/+tvrjp3hH+4qoX9IJmJxQZ3M85bkHZB2u9Ov65a6uaoVA==", "55ea2ead-d503-4811-86ef-26e60adf6391", new DateTime(2024, 2, 6, 11, 35, 37, 467, DateTimeKind.Local).AddTicks(3119) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "c8b3d473-b329-4315-95b7-e296cebaa2ad", new DateTime(2024, 2, 6, 11, 33, 31, 445, DateTimeKind.Local).AddTicks(4240), "AQAAAAEAACcQAAAAEPbgGVRH4WnlKqek3+7b0A/s7+lO2Klg5mFa/ZuXLvU71LS5LihBz1qSSmhJM3ejcg==", "a72051aa-d01d-49fc-9b10-fa27fcbb1201", new DateTime(2024, 2, 6, 11, 33, 31, 445, DateTimeKind.Local).AddTicks(4251) });
        }
    }
}
