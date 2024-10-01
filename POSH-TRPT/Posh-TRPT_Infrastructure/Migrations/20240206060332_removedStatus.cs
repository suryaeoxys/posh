using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class removedStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "StatusId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "StatusId", "UpdatedDate" },
                values: new object[] { "c8b3d473-b329-4315-95b7-e296cebaa2ad", new DateTime(2024, 2, 6, 11, 33, 31, 445, DateTimeKind.Local).AddTicks(4240), "AQAAAAEAACcQAAAAEPbgGVRH4WnlKqek3+7b0A/s7+lO2Klg5mFa/ZuXLvU71LS5LihBz1qSSmhJM3ejcg==", "a72051aa-d01d-49fc-9b10-fa27fcbb1201", null, new DateTime(2024, 2, 6, 11, 33, 31, 445, DateTimeKind.Local).AddTicks(4251) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "StatusId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "StatusId", "UpdatedDate" },
                values: new object[] { "493a9f9c-62a3-4c0d-8e36-817f3c5a42ae", new DateTime(2024, 2, 6, 11, 31, 50, 126, DateTimeKind.Local).AddTicks(5059), "AQAAAAEAACcQAAAAEOApeVZrMr6H3SPAeXI7YmOWgvUZkLXV6FqnGxJF69i0fCRZ7TkrBeqKqk69Ng4k8g==", "f3788f7c-3e42-4512-b05d-8c33beab8b99", new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 2, 6, 11, 31, 50, 126, DateTimeKind.Local).AddTicks(5067) });
        }
    }
}
