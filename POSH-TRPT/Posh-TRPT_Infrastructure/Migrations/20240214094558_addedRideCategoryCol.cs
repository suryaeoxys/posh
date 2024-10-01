using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class addedRideCategoryCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RideCategoryId",
                table: "TblVehicleDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "61abf6e5-2a7c-4610-a2ca-abbe139dab97", new DateTime(2024, 2, 14, 15, 15, 57, 95, DateTimeKind.Local).AddTicks(8611), "AQAAAAEAACcQAAAAEC0/TCtYwCZlB+JOyRu30xQPkys8tL+s7izY1ns1B8qMVZccEYfrtJbdVdvuEseVOQ==", "8c30ede0-a9a1-4bb5-88af-f30389260d89", new DateTime(2024, 2, 14, 15, 15, 57, 95, DateTimeKind.Local).AddTicks(8623) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RideCategoryId",
                table: "TblVehicleDetails");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "b76d8566-aa82-45ec-a31a-b055c8dad272", new DateTime(2024, 2, 8, 17, 8, 37, 905, DateTimeKind.Local).AddTicks(2738), "AQAAAAEAACcQAAAAEFOaPPWmOrICflvxnBRXkZwXhqe4dmv9w28xAyV6EhJfc3HmZzy5Lo40BkzUU1hy4w==", "aaac1b69-1168-454c-9516-b6ef9b5f1ed2", new DateTime(2024, 2, 8, 17, 8, 37, 905, DateTimeKind.Local).AddTicks(2746) });
        }
    }
}
