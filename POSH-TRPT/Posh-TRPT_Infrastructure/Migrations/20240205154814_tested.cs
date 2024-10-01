using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class tested : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "3f6307ed-34f1-457b-9265-a490606cfc57", new DateTime(2024, 2, 5, 21, 18, 12, 997, DateTimeKind.Local).AddTicks(4015), "AQAAAAEAACcQAAAAEEYwoLvOLOdN81xDAwNQSTxD8C10SEj9dqdaTY31kXoi/Jwd0Z69xHZTZ+J1/KyQhw==", "aaaf7f5b-4837-4d8c-9d5b-76a6545edf45", new DateTime(2024, 2, 5, 21, 18, 12, 997, DateTimeKind.Local).AddTicks(4025) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "289a423c-0609-4032-88f3-847edd68f218", new DateTime(2024, 2, 2, 14, 51, 1, 533, DateTimeKind.Local).AddTicks(3053), "AQAAAAEAACcQAAAAENwLh5aVbw2PFklnPaTK74jFMcPfY65RrLcBIFPQKqI17tk5l45erSq+oxUUnWSlPg==", "dfab572f-758c-41c5-8b80-fd1961f3edc4", new DateTime(2024, 2, 2, 14, 51, 1, 533, DateTimeKind.Local).AddTicks(3062) });
        }
    }
}
