using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedLongitudeLatitudeASPNETUSER : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "AspNetUsers",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "0f61468e-68bc-478f-aa8a-0e106b3853a0", new DateTime(2024, 2, 28, 16, 45, 16, 477, DateTimeKind.Local).AddTicks(7305), "AQAAAAEAACcQAAAAECjO12JjB4g9bS8y4VL8k2Stkqu5wjyMzV+mX2h0hYWt1Mg9/GeH9aLvb1I7G6+V5A==", "2813a77d-cb4a-44ad-93c6-7f0524ce23a9", new DateTime(2024, 2, 28, 16, 45, 16, 477, DateTimeKind.Local).AddTicks(7315) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "61abf6e5-2a7c-4610-a2ca-abbe139dab97", new DateTime(2024, 2, 14, 15, 15, 57, 95, DateTimeKind.Local).AddTicks(8611), "AQAAAAEAACcQAAAAEC0/TCtYwCZlB+JOyRu30xQPkys8tL+s7izY1ns1B8qMVZccEYfrtJbdVdvuEseVOQ==", "8c30ede0-a9a1-4bb5-88af-f30389260d89", new DateTime(2024, 2, 14, 15, 15, 57, 95, DateTimeKind.Local).AddTicks(8623) });
        }
    }
}
