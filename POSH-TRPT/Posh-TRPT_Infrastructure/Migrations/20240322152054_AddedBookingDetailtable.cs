using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedBookingDetailtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "dc4170a4-77c0-4623-b5ee-d2547b772ce1", new DateTime(2024, 3, 22, 20, 50, 54, 85, DateTimeKind.Local).AddTicks(906), "AQAAAAEAACcQAAAAEAYwRtkZ97xkUiQ/OkLPiqGl8RFtyp8K1PyPvtEiwIDQtXjdqyrm+f5xJLolWBIq8g==", "a975d4bc-83eb-4069-bfda-cdbcbf11d4d5", new DateTime(2024, 3, 22, 20, 50, 54, 85, DateTimeKind.Local).AddTicks(914) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "4f4eff22-0319-4929-ba91-7dd236f82ac2", new DateTime(2024, 3, 19, 15, 49, 23, 898, DateTimeKind.Local).AddTicks(3308), "AQAAAAEAACcQAAAAEGcJb1IzYtw/cshlGJkoKAJVeQhde4fnbgloZO1LW0T5oW9H0bge2XyulGrMRvon3w==", "7e748d7a-0c37-4973-bd71-5d9f27552907", new DateTime(2024, 3, 19, 15, 49, 23, 898, DateTimeKind.Local).AddTicks(3316) });
        }
    }
}
