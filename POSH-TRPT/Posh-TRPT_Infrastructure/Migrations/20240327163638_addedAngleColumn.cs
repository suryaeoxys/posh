using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class addedAngleColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Angle",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "Angle", "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { 0.0, "c204e4d0-92d9-402c-9505-e4d69530bc66", new DateTime(2024, 3, 27, 22, 6, 38, 267, DateTimeKind.Local).AddTicks(2929), "AQAAAAEAACcQAAAAELnMecDE8owcapTNo6yTVoB64hJ3TOkDN8Zh4JxxDDyQi9OeSZgt010WeEw14RagFw==", "ed69fcfb-5308-4202-8821-e9821a1d32d5", new DateTime(2024, 3, 27, 22, 6, 38, 267, DateTimeKind.Local).AddTicks(2937) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Angle",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "f182c32e-729a-4503-a66e-ad9e7ae96804", new DateTime(2024, 3, 27, 19, 29, 26, 455, DateTimeKind.Local).AddTicks(2819), "AQAAAAEAACcQAAAAEGZzvxeYhfOTTE0LKRsYcsLJn/lqemK5q3jdrsfqKjizNcgLrbsV8HuG9MNYFIq8HQ==", "011ef05a-f90a-4c53-ab8f-1337da188066", new DateTime(2024, 3, 27, 19, 29, 26, 455, DateTimeKind.Local).AddTicks(2828) });
        }
    }
}
