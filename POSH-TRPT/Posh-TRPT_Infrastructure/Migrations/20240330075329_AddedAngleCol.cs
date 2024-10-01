using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedAngleCol : Migration
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
                values: new object[] { 0.0, "ed322fbb-4f95-48f7-b8c9-d1a30dba44e5", new DateTime(2024, 3, 30, 13, 23, 29, 12, DateTimeKind.Local).AddTicks(2849), "AQAAAAEAACcQAAAAEADEjAbUuGnQ//L7aJQM9B8JyuzPIr9QX5gXTFIj1QVKDqoTxTQx7gHiV0hzQgh6Gw==", "821356a1-3d0a-4ee9-8ab6-d8f5f98a4550", new DateTime(2024, 3, 30, 13, 23, 29, 12, DateTimeKind.Local).AddTicks(2857) });
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
                values: new object[] { "e48f1863-ae73-4a0d-8f6e-6e9549d19670", new DateTime(2024, 3, 28, 20, 48, 8, 122, DateTimeKind.Local).AddTicks(4424), "AQAAAAEAACcQAAAAED+6Za9pP8i2Ws4BgglJz8h9GKz6A/53iagKiFa6sBrWviDibsaMrj/JfyWjnPyYYw==", "ad7b59d7-ed02-44db-89f4-88ace64ff5a5", new DateTime(2024, 3, 28, 20, 48, 8, 122, DateTimeKind.Local).AddTicks(4433) });
        }
    }
}
