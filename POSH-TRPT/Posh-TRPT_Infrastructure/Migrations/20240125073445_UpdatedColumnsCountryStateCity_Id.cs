using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class UpdatedColumnsCountryStateCity_Id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "6ec37bf1-fe6d-4ba4-b6da-66a22119fce0", new DateTime(2024, 1, 25, 13, 4, 44, 77, DateTimeKind.Local).AddTicks(8205), "AQAAAAEAACcQAAAAEFJsMG/r+fAw676pxqMVb8hPiQysz/UshnJZial4j+bWwdy2gIFYhgG/OKrlwrW4uA==", "9bcfad32-744c-491b-9def-d0acd70e4613", new DateTime(2024, 1, 25, 13, 4, 44, 77, DateTimeKind.Local).AddTicks(8217) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "4ccdc7f2-8ca2-48db-aa27-dd2641267138", new DateTime(2024, 1, 24, 18, 19, 50, 535, DateTimeKind.Local).AddTicks(8932), "AQAAAAEAACcQAAAAEEfzEzMFxYog0dyd7/0zXpY/RnKK1IBsQ6ztnlAmT529vOt9qShAYBOThnv2LsHLEg==", "e9b79827-5597-4565-90ce-26890b2b6b83", new DateTime(2024, 1, 24, 18, 19, 50, 535, DateTimeKind.Local).AddTicks(8941) });
        }
    }
}
