using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedSNNColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Social_Security_Number",
                table: "TblDriverDocuments");

            migrationBuilder.AddColumn<int>(
                name: "Sequence",
                table: "TblMenuMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Social_Security_Number",
                table: "TblGeneralAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "452e50a3-15bb-47f7-8754-16b9712b5daa", new DateTime(2024, 1, 26, 2, 48, 30, 686, DateTimeKind.Local).AddTicks(3552), "AQAAAAEAACcQAAAAEBw/dEcVcbzrbBgrN+qbxph3QaU/UmP6wWRm9kGVoWnwUcHFz1USy3ihZcbGVEPUCg==", "5f1b02f6-858a-426b-9025-7c73f4135bc6", new DateTime(2024, 1, 26, 2, 48, 30, 686, DateTimeKind.Local).AddTicks(3561) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "TblMenuMaster");

            migrationBuilder.DropColumn(
                name: "Social_Security_Number",
                table: "TblGeneralAddress");

            migrationBuilder.AddColumn<string>(
                name: "Social_Security_Number",
                table: "TblDriverDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "6ec37bf1-fe6d-4ba4-b6da-66a22119fce0", new DateTime(2024, 1, 25, 13, 4, 44, 77, DateTimeKind.Local).AddTicks(8205), "AQAAAAEAACcQAAAAEFJsMG/r+fAw676pxqMVb8hPiQysz/UshnJZial4j+bWwdy2gIFYhgG/OKrlwrW4uA==", "9bcfad32-744c-491b-9def-d0acd70e4613", new DateTime(2024, 1, 25, 13, 4, 44, 77, DateTimeKind.Local).AddTicks(8217) });
        }
    }
}
