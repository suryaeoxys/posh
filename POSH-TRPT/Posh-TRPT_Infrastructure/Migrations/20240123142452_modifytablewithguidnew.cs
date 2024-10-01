using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class modifytablewithguidnew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "TblMenuMaster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "TblMenuMaster",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "TblMenuMaster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "TblMenuMaster",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Tbl_States",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Tbl_States",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Tbl_States",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Tbl_States",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Tbl_Countries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Tbl_Countries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Tbl_Countries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Tbl_Countries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Tbl_Cities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Tbl_Cities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Tbl_Cities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Tbl_Cities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "66e5ac3a-527c-4fb4-94e1-e27960c61d40", new DateTime(2024, 1, 23, 19, 54, 52, 513, DateTimeKind.Local).AddTicks(1466), "AQAAAAEAACcQAAAAEERBfXQvdRhHfclMdAEuNYl8ntjCoV8SsUXeefsYE3U7vq5Sa7xz1c3tI0Y9c5vZDQ==", "7fb76a8f-0f92-436f-95f2-01bfcbde5e16", new DateTime(2024, 1, 23, 19, 54, 52, 513, DateTimeKind.Local).AddTicks(1474) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TblMenuMaster");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "TblMenuMaster");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "TblMenuMaster");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "TblMenuMaster");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Tbl_States");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Tbl_States");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Tbl_States");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Tbl_States");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Tbl_Countries");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Tbl_Countries");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Tbl_Countries");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Tbl_Countries");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Tbl_Cities");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Tbl_Cities");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Tbl_Cities");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Tbl_Cities");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "d863cdf6-5040-41d7-8ca6-3253593e3f7e", new DateTime(2024, 1, 23, 19, 52, 39, 203, DateTimeKind.Local).AddTicks(7742), "AQAAAAEAACcQAAAAEJBjuw9vIMRz3ExYsnKyWmtT7dselVElu5IwyMozYVW4acTYhPuIFdVNBFQH3kCLtw==", "a93061fb-5eb7-47d4-8bf7-59d7c36336f6", new DateTime(2024, 1, 23, 19, 52, 39, 203, DateTimeKind.Local).AddTicks(7755) });
        }
    }
}
