using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class ForeignKeyForDocAndAddressTabANdMenuTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TblGeneralAddress",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "TblDriverDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "TblDriverDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TblDriverDocuments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Tbl_States",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Tbl_States",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Tbl_Countries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Tbl_Countries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Tbl_Cities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Tbl_Cities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TblMenuMaster",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MenuID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MenuName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Parent_MenuID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User_Roll = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MenuFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MenuURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    USE_YN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblMenuMaster", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "c2e97507-3c04-4438-9651-02352b6670a7", new DateTime(2024, 1, 12, 20, 49, 52, 363, DateTimeKind.Local).AddTicks(2135), "AQAAAAEAACcQAAAAEH92msaRHNKdfQoDX46n8kJvh6++cHHOuG0ZF6WI57HNYM46coV5Dz6kXU57pNuqFw==", "86dd7360-633a-4d39-8735-7d89a3b8a9d7", new DateTime(2024, 1, 12, 20, 49, 52, 363, DateTimeKind.Local).AddTicks(2147) });

            migrationBuilder.CreateIndex(
                name: "IX_TblGeneralAddress_UserId",
                table: "TblGeneralAddress",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TblDriverDocuments_UserId",
                table: "TblDriverDocuments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TblDriverDocuments_AspNetUsers_UserId",
                table: "TblDriverDocuments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TblGeneralAddress_AspNetUsers_UserId",
                table: "TblGeneralAddress",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblDriverDocuments_AspNetUsers_UserId",
                table: "TblDriverDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_TblGeneralAddress_AspNetUsers_UserId",
                table: "TblGeneralAddress");

            migrationBuilder.DropTable(
                name: "TblMenuMaster");

            migrationBuilder.DropIndex(
                name: "IX_TblGeneralAddress_UserId",
                table: "TblGeneralAddress");

            migrationBuilder.DropIndex(
                name: "IX_TblDriverDocuments_UserId",
                table: "TblDriverDocuments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TblGeneralAddress");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TblDriverDocuments");

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

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "TblDriverDocuments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "TblDriverDocuments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Tbl_States",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Tbl_States",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Tbl_Countries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Tbl_Countries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Tbl_Cities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Tbl_Cities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "fff47497-2e96-4f07-b6d5-80c55ee0f1e3", new DateTime(2024, 1, 11, 18, 18, 2, 664, DateTimeKind.Local).AddTicks(3486), "AQAAAAEAACcQAAAAEGn/rDIjVQwGZ10RERAbGFdccUt5dYrDvarr3ngWeyGWn4AZJTazwVH6Td0u0Ut4Lw==", "a538cb60-c592-413b-9004-8b1726dd2f9d", new DateTime(2024, 1, 11, 18, 18, 2, 664, DateTimeKind.Local).AddTicks(3494) });
        }
    }
}
