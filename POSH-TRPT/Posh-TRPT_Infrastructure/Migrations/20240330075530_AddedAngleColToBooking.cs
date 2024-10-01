using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedAngleColToBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Angle",
                table: "TblBookingDetail",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "Angle","ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] {0.0, "31aeead5-ca29-402e-b62a-ae46c01f7476", new DateTime(2024, 3, 30, 13, 25, 29, 649, DateTimeKind.Local).AddTicks(9124), "AQAAAAEAACcQAAAAECsRqbrfpLUxSeExcyeBAzkVYUjlwbnxuhdTHljLdHocJaz/4kUtj4aMY7MpyRHR1Q==", "4429effe-65b6-48f4-9e08-f16d33ffeeca", new DateTime(2024, 3, 30, 13, 25, 29, 649, DateTimeKind.Local).AddTicks(9133) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Angle",
                table: "TblBookingDetail");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "ed322fbb-4f95-48f7-b8c9-d1a30dba44e5", new DateTime(2024, 3, 30, 13, 23, 29, 12, DateTimeKind.Local).AddTicks(2849), "AQAAAAEAACcQAAAAEADEjAbUuGnQ//L7aJQM9B8JyuzPIr9QX5gXTFIj1QVKDqoTxTQx7gHiV0hzQgh6Gw==", "821356a1-3d0a-4ee9-8ab6-d8f5f98a4550", new DateTime(2024, 3, 30, 13, 23, 29, 12, DateTimeKind.Local).AddTicks(2857) });
        }
    }
}
