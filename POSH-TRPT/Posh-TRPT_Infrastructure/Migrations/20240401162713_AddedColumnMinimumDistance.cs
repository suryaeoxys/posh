using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedColumnMinimumDistance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MinimumDistance",
                table: "TblBookingDetail",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "d34490bf-72b2-41d6-b490-da1d3dedda36", new DateTime(2024, 4, 1, 21, 57, 12, 959, DateTimeKind.Local).AddTicks(8869), "AQAAAAEAACcQAAAAEEBRAuGvUzVHqeUiegm78r7Lx4sv2X1Pm1F2RSBhwa4LJU9iGmwfYxz7i4PqsL8W0w==", "0927adec-2954-49b4-8790-05259b225465", new DateTime(2024, 4, 1, 21, 57, 12, 959, DateTimeKind.Local).AddTicks(8880) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinimumDistance",
                table: "TblBookingDetail");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "31aeead5-ca29-402e-b62a-ae46c01f7476", new DateTime(2024, 3, 30, 13, 25, 29, 649, DateTimeKind.Local).AddTicks(9124), "AQAAAAEAACcQAAAAECsRqbrfpLUxSeExcyeBAzkVYUjlwbnxuhdTHljLdHocJaz/4kUtj4aMY7MpyRHR1Q==", "4429effe-65b6-48f4-9e08-f16d33ffeeca", new DateTime(2024, 3, 30, 13, 25, 29, 649, DateTimeKind.Local).AddTicks(9133) });
        }
    }
}
