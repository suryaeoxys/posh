using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class addedNewColumnsToStripe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblStripeCustomers_AspNetUsers_UserId",
                table: "TblStripeCustomers");

            migrationBuilder.DropIndex(
                name: "IX_TblStripeCustomers_UserId",
                table: "TblStripeCustomers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TblStripeCustomers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EphemeralCreatedDate",
                table: "TblStripeCustomers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EphemeralExpiresDate",
                table: "TblStripeCustomers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EphemeralKey",
                table: "TblStripeCustomers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EphemeralSecret",
                table: "TblStripeCustomers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "01dd5e5a-c15b-4263-b05e-54afcdb6e9e1", new DateTime(2024, 4, 19, 16, 8, 45, 881, DateTimeKind.Local).AddTicks(1533), "AQAAAAEAACcQAAAAEEhC3PV5ewst60XZvyhFzwVNaiDWe7jL1Cg3cet+/dIUJ9XA5ojk4vvqu/5FUAtkew==", "4dad4768-12f2-4ec2-b32e-d8753df143bd", new DateTime(2024, 4, 19, 16, 8, 45, 881, DateTimeKind.Local).AddTicks(1543) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EphemeralCreatedDate",
                table: "TblStripeCustomers");

            migrationBuilder.DropColumn(
                name: "EphemeralExpiresDate",
                table: "TblStripeCustomers");

            migrationBuilder.DropColumn(
                name: "EphemeralKey",
                table: "TblStripeCustomers");

            migrationBuilder.DropColumn(
                name: "EphemeralSecret",
                table: "TblStripeCustomers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TblStripeCustomers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "3564806f-63fe-46ad-bd23-aec8ac70eb31", new DateTime(2024, 4, 17, 22, 7, 54, 468, DateTimeKind.Local).AddTicks(4336), "AQAAAAEAACcQAAAAEPG6lDLkIIUYdJpTsfRP4hnoMq8CenNx88NU8VKu+mkdY5EpMcHubrypxcacg4sTxg==", "87e6f48a-927f-4421-8bfd-884eb2484695", new DateTime(2024, 4, 17, 22, 7, 54, 468, DateTimeKind.Local).AddTicks(4354) });

            migrationBuilder.CreateIndex(
                name: "IX_TblStripeCustomers_UserId",
                table: "TblStripeCustomers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TblStripeCustomers_AspNetUsers_UserId",
                table: "TblStripeCustomers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
