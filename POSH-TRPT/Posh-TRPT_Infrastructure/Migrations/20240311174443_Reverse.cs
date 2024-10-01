using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class Reverse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "2a7a732c-e682-4ff0-8255-4b3b094e780b", new DateTime(2024, 3, 11, 23, 14, 43, 362, DateTimeKind.Local).AddTicks(2674), "AQAAAAEAACcQAAAAEJo2/4Q42dVuDkbqvGj7TBM53FMeguo/nhKeGObbh0NpxkX7IjvyDwuCR6+jPgcbVQ==", "17c0a5c0-681a-4361-ab92-d004f7eb92ba", new DateTime(2024, 3, 11, 23, 14, 43, 362, DateTimeKind.Local).AddTicks(2686) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "f24d3ba0-0980-4cc8-b402-2d80bc9511c3", new DateTime(2024, 3, 11, 22, 25, 55, 390, DateTimeKind.Local).AddTicks(1723), "AQAAAAEAACcQAAAAEEqQfFw2jZXzLDECFNFWPDI67tQdbOp9/wKfiuvVnLQPXVC+clOJSrYQdJdtMpVWqg==", "7c97dcad-d854-4d91-8609-4e57f10555c2", new DateTime(2024, 3, 11, 22, 25, 55, 390, DateTimeKind.Local).AddTicks(1731) });
        }
    }
}
