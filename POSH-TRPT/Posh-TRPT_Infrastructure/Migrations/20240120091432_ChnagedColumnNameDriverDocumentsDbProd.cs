using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class ChnagedColumnNameDriverDocumentsDbProd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "32d681fb-1ed9-4c7c-90cd-5412d8507b20", new DateTime(2024, 1, 20, 14, 44, 32, 549, DateTimeKind.Local).AddTicks(266), "AQAAAAEAACcQAAAAEHz6V61iuLGKhDvdKJS526UKCTrb0IhG6LQQg2RTVnbRjxB3X3sQjq+CdyR2k67NFA==", "ba876019-7c0a-4b20-9eda-298dbac732ec", new DateTime(2024, 1, 20, 14, 44, 32, 549, DateTimeKind.Local).AddTicks(274) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "2b8aaf0b-efe5-4c6b-8177-c3457d0dc213", new DateTime(2024, 1, 20, 14, 42, 44, 432, DateTimeKind.Local).AddTicks(2796), "AQAAAAEAACcQAAAAEIW9LA701/YwC/5m/3ZUNYs5+qMhYISoMLdJlrwMlX7KWafYeEL4qDD23dhTOVCE/w==", "e87108f4-8c39-407c-a04f-1845cd872c02", new DateTime(2024, 1, 20, 14, 42, 44, 432, DateTimeKind.Local).AddTicks(2805) });
        }
    }
}
