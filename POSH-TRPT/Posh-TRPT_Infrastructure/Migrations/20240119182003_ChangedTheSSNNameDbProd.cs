using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class ChangedTheSSNNameDbProd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "57d3419c-adbb-42cf-be06-0f026063ac48", new DateTime(2024, 1, 19, 23, 50, 3, 410, DateTimeKind.Local).AddTicks(1663), "AQAAAAEAACcQAAAAEF9ZiIdLs8SjQDXQ0ZEWzP91IFNZ8HoAMo6q9secR+2Tof7i9KMOvSDcIcKs4tCFdA==", "a212d11d-3575-47de-b7cf-ad7b4932105c", new DateTime(2024, 1, 19, 23, 50, 3, 410, DateTimeKind.Local).AddTicks(1672) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "a082021d-da56-4b69-a62f-282899fd7000", new DateTime(2024, 1, 19, 23, 48, 13, 74, DateTimeKind.Local).AddTicks(5960), "AQAAAAEAACcQAAAAEAgKIi6z8zIxJCNFqIXfimUYSku8/qfNbqEZzkMXQOa16Y1k+Q5ZGIfycwyBll44Ig==", "3763b8be-7041-46ad-be95-faec57674e5e", new DateTime(2024, 1, 19, 23, 48, 13, 74, DateTimeKind.Local).AddTicks(5968) });
        }
    }
}
