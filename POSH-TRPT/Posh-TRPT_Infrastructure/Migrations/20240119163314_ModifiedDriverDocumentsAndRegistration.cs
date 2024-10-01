using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class ModifiedDriverDocumentsAndRegistration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "731a29ca-303e-4960-8a94-b28039b9e49a", new DateTime(2024, 1, 19, 22, 3, 14, 452, DateTimeKind.Local).AddTicks(499), "AQAAAAEAACcQAAAAEBI1nuDA35JFwcEne8MNKBhfIniVkToHLp9U8U+G5cKfDAA3Ts8nt7gLgvPf4WB+cA==", "fceeca4a-ace2-4807-aff4-8090caa2622e", new DateTime(2024, 1, 19, 22, 3, 14, 452, DateTimeKind.Local).AddTicks(507) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "ebadb049-46bb-4f30-a46b-8d1c1b642d89", new DateTime(2024, 1, 19, 18, 24, 17, 568, DateTimeKind.Local).AddTicks(4074), "AQAAAAEAACcQAAAAED4vzxnjERekAi+PfhbSMfjTo++0rRza4FCuGupVsSYSakhftovL3Th4jYfH0RZLwQ==", "126d3c5e-1321-49c9-b9b7-1f2eadc6ae14", new DateTime(2024, 1, 19, 18, 24, 17, 568, DateTimeKind.Local).AddTicks(4087) });
        }
    }
}
