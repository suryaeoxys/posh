using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class removedStatusFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_TblStatus_StatusId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_StatusId",
                table: "AspNetUsers");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                values: new object[] { "3f6307ed-34f1-457b-9265-a490606cfc57", new DateTime(2024, 2, 5, 21, 18, 12, 997, DateTimeKind.Local).AddTicks(4015), "AQAAAAEAACcQAAAAEEYwoLvOLOdN81xDAwNQSTxD8C10SEj9dqdaTY31kXoi/Jwd0Z69xHZTZ+J1/KyQhw==", "aaaf7f5b-4837-4d8c-9d5b-76a6545edf45", null, new DateTime(2024, 2, 5, 21, 18, 12, 997, DateTimeKind.Local).AddTicks(4025) });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_StatusId",
                table: "AspNetUsers",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_TblStatus_StatusId",
                table: "AspNetUsers",
                column: "StatusId",
                principalTable: "TblStatus",
                principalColumn: "Id");
        }
    }
}
