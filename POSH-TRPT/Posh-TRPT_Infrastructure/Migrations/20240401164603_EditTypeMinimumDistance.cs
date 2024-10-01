using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class EditTypeMinimumDistance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MinimumDistance",
                table: "TblBookingDetail",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "17cf80ac-0531-4bc3-9482-983571580d5b", new DateTime(2024, 4, 1, 22, 16, 3, 462, DateTimeKind.Local).AddTicks(4090), "AQAAAAEAACcQAAAAEKQoMdA8YJDPuZR5QUUdbO/AU0LW61UNKxgsOGRrUsnSWf+NMoB4SpxZ9WRi2Og1YA==", "8729038d-2f22-493a-a8ee-cc785a68373c", new DateTime(2024, 4, 1, 22, 16, 3, 462, DateTimeKind.Local).AddTicks(4100) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "MinimumDistance",
                table: "TblBookingDetail",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "d34490bf-72b2-41d6-b490-da1d3dedda36", new DateTime(2024, 4, 1, 21, 57, 12, 959, DateTimeKind.Local).AddTicks(8869), "AQAAAAEAACcQAAAAEEBRAuGvUzVHqeUiegm78r7Lx4sv2X1Pm1F2RSBhwa4LJU9iGmwfYxz7i4PqsL8W0w==", "0927adec-2954-49b4-8790-05259b225465", new DateTime(2024, 4, 1, 21, 57, 12, 959, DateTimeKind.Local).AddTicks(8880) });
        }
    }
}
