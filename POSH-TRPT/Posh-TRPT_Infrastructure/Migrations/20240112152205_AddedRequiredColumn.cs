using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedRequiredColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "TblGeneralAddress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "TblGeneralAddress",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "TblGeneralAddress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "TblGeneralAddress",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "1a2e516a-5dcd-43b5-a8cb-2ad8c612ac58", new DateTime(2024, 1, 12, 20, 52, 5, 540, DateTimeKind.Local).AddTicks(9857), "AQAAAAEAACcQAAAAEOZz6LJVxBdrqQtEhJ/YhCFjKBCO645eENP8Psw4NyCCB9o9U3zKdcPC4DPAtuulJA==", "238457ae-7180-4d90-928e-7126d5da8b0c", new DateTime(2024, 1, 12, 20, 52, 5, 540, DateTimeKind.Local).AddTicks(9866) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TblGeneralAddress");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "TblGeneralAddress");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "TblGeneralAddress");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "TblGeneralAddress");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "c2e97507-3c04-4438-9651-02352b6670a7", new DateTime(2024, 1, 12, 20, 49, 52, 363, DateTimeKind.Local).AddTicks(2135), "AQAAAAEAACcQAAAAEH92msaRHNKdfQoDX46n8kJvh6++cHHOuG0ZF6WI57HNYM46coV5Dz6kXU57pNuqFw==", "86dd7360-633a-4d39-8735-7d89a3b8a9d7", new DateTime(2024, 1, 12, 20, 49, 52, 363, DateTimeKind.Local).AddTicks(2147) });
        }
    }
}
