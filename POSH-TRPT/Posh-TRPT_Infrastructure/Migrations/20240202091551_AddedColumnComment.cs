using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedColumnComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "445fc051-1e0b-43c9-9bd0-e82d2b4227b4", new DateTime(2024, 2, 2, 14, 45, 50, 307, DateTimeKind.Local).AddTicks(6294), "AQAAAAEAACcQAAAAEEqT3Xmb/ODui5/LBaPJWPNCbI251KaRSiEv3s34VwENglpZULEGzqyCqdg+7LT+tw==", "d4e7f37e-536f-4e1f-b72e-857b5cdbcf46", new DateTime(2024, 2, 2, 14, 45, 50, 307, DateTimeKind.Local).AddTicks(6303) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "8a9f3411-a519-4ebc-9f67-490482f8cad1", new DateTime(2024, 2, 1, 14, 50, 53, 728, DateTimeKind.Local).AddTicks(8785), "AQAAAAEAACcQAAAAEKTaIQbZ6y4P7evwQdM9tPKeonnJgwD8SC7HKLBtFfyWRQOe03V6r99wImzdljDe2Q==", "988c283f-26cd-4723-9115-06d6713cfe9c", new DateTime(2024, 2, 1, 14, 50, 53, 728, DateTimeKind.Local).AddTicks(8794) });
        }
    }
}
