using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedColumnCommentDbProd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "289a423c-0609-4032-88f3-847edd68f218", new DateTime(2024, 2, 2, 14, 51, 1, 533, DateTimeKind.Local).AddTicks(3053), "AQAAAAEAACcQAAAAENwLh5aVbw2PFklnPaTK74jFMcPfY65RrLcBIFPQKqI17tk5l45erSq+oxUUnWSlPg==", "dfab572f-758c-41c5-8b80-fd1961f3edc4", new DateTime(2024, 2, 2, 14, 51, 1, 533, DateTimeKind.Local).AddTicks(3062) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "445fc051-1e0b-43c9-9bd0-e82d2b4227b4", new DateTime(2024, 2, 2, 14, 45, 50, 307, DateTimeKind.Local).AddTicks(6294), "AQAAAAEAACcQAAAAEEqT3Xmb/ODui5/LBaPJWPNCbI251KaRSiEv3s34VwENglpZULEGzqyCqdg+7LT+tw==", "d4e7f37e-536f-4e1f-b72e-857b5cdbcf46", new DateTime(2024, 2, 2, 14, 45, 50, 307, DateTimeKind.Local).AddTicks(6303) });
        }
    }
}
