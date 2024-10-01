using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class IsBackgroundCheckedAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Social_Security_Numbar",
                table: "TblDriverDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsBackgroundChecked",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "395d0a83-ecfc-421b-b0e8-9e17e6628523", new DateTime(2024, 1, 17, 20, 39, 47, 502, DateTimeKind.Local).AddTicks(4392), "AQAAAAEAACcQAAAAEJdgKUCu8E4+vB0BPAaOESolqa6+MKfBnMRak7hJwSDsfXPrbjbJo8pcmmlxHi6csg==", "a1d06178-2f81-460f-bb3e-e1e30d1dddb1", new DateTime(2024, 1, 17, 20, 39, 47, 502, DateTimeKind.Local).AddTicks(4406) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Social_Security_Numbar",
                table: "TblDriverDocuments");

            migrationBuilder.DropColumn(
                name: "IsBackgroundChecked",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "489d3254-5bb5-448a-994b-6adf8a8bf882", new DateTime(2024, 1, 15, 23, 44, 34, 151, DateTimeKind.Local).AddTicks(254), "AQAAAAEAACcQAAAAEI4gxgnlTmGlhdpj9Nat8XrON93jb8EkmeWeCt7rpAgbPIlEo9WepNOb+ej0l8d6Ug==", "d6ae4a62-a400-42c9-bb07-1afbc1b6e0d8", new DateTime(2024, 1, 15, 23, 44, 34, 151, DateTimeKind.Local).AddTicks(263) });
        }
    }
}
