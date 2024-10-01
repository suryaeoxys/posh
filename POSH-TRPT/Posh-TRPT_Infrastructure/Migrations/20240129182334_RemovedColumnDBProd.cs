using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class RemovedColumnDBProd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "67c852a4-72d6-4f84-b486-dab876d3563f", new DateTime(2024, 1, 29, 23, 53, 33, 759, DateTimeKind.Local).AddTicks(7673), "AQAAAAEAACcQAAAAEITH98dZMjjzBdhBByfLkz/K5sy9Z3F5VXQxm/5ww/KuNXmv1umXUiZiyX3efnLqUw==", "c7b8fc95-fd3f-4ae9-9ddf-356d042fe41c", new DateTime(2024, 1, 29, 23, 53, 33, 759, DateTimeKind.Local).AddTicks(7686) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "263113d9-9d36-4e07-9c7e-54f0d078c9bb", new DateTime(2024, 1, 29, 19, 34, 18, 443, DateTimeKind.Local).AddTicks(4795), "AQAAAAEAACcQAAAAEPtTKqJFXR+m1gO8ujcqnCQ4C6n6CIdm7135gndhL3rUGRmLEFQMZqcYElIy95hEng==", "bb68ffbf-e6a8-4bee-aea6-7342b8b1b4b9", new DateTime(2024, 1, 29, 19, 34, 18, 443, DateTimeKind.Local).AddTicks(4805) });
        }
    }
}
