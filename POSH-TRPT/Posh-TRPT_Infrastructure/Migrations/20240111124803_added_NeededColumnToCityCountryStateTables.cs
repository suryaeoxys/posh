using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class added_NeededColumnToCityCountryStateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "TblGeneralAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "TblGeneralAddress",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TblGeneralAddress",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "TblGeneralAddress",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "TblGeneralAddress",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "TblDriverDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "TblDriverDocuments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TblDriverDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "TblDriverDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "TblDriverDocuments",
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

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Tbl_States",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Tbl_Countries",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Tbl_Cities",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
                values: new object[] { "fff47497-2e96-4f07-b6d5-80c55ee0f1e3", new DateTime(2024, 1, 11, 18, 18, 2, 664, DateTimeKind.Local).AddTicks(3486), "AQAAAAEAACcQAAAAEGn/rDIjVQwGZ10RERAbGFdccUt5dYrDvarr3ngWeyGWn4AZJTazwVH6Td0u0Ut4Lw==", "a538cb60-c592-413b-9004-8b1726dd2f9d", new DateTime(2024, 1, 11, 18, 18, 2, 664, DateTimeKind.Local).AddTicks(3494) });
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
                name: "IsDeleted",
                table: "TblGeneralAddress");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "TblGeneralAddress");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "TblGeneralAddress");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TblDriverDocuments");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "TblDriverDocuments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TblDriverDocuments");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "TblDriverDocuments");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "TblDriverDocuments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Tbl_States");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Tbl_States");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
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
                name: "IsDeleted",
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
                name: "IsDeleted",
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
                values: new object[] { "afa82607-471d-4f68-8644-e8d58dc03e17", new DateTime(2024, 1, 8, 15, 11, 34, 156, DateTimeKind.Local).AddTicks(151), "AQAAAAEAACcQAAAAEBCT18sSLxP0sh8tLOsnDUj9j4CDw9jYnUu4WW88W2O2IpDwMm6Gmt+pDm62UQXxXA==", "6c773ab5-d58b-4d97-9a80-f8a073a3b459", new DateTime(2024, 1, 8, 15, 11, 34, 156, DateTimeKind.Local).AddTicks(163) });
        }
    }
}
