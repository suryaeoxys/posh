using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class modifytblIsActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TblMenuMaster",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Tbl_States",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Tbl_Countries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Tbl_Cities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "828a33b3-bc38-471c-9437-d520d1e2f13d", new DateTime(2024, 1, 23, 19, 55, 56, 958, DateTimeKind.Local).AddTicks(4466), "AQAAAAEAACcQAAAAEDa94G3QipI0xsNDTclxtSWRuN8X1bFvGXF2/Xl3RFgsMknonHMKo41Ui/gb29mYkQ==", "ef5f6c53-af82-4273-aaed-d735e8620e57", new DateTime(2024, 1, 23, 19, 55, 56, 958, DateTimeKind.Local).AddTicks(4478) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TblMenuMaster");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Tbl_States");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Tbl_Countries");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Tbl_Cities");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "66e5ac3a-527c-4fb4-94e1-e27960c61d40", new DateTime(2024, 1, 23, 19, 54, 52, 513, DateTimeKind.Local).AddTicks(1466), "AQAAAAEAACcQAAAAEERBfXQvdRhHfclMdAEuNYl8ntjCoV8SsUXeefsYE3U7vq5Sa7xz1c3tI0Y9c5vZDQ==", "7fb76a8f-0f92-436f-95f2-01bfcbde5e16", new DateTime(2024, 1, 23, 19, 54, 52, 513, DateTimeKind.Local).AddTicks(1474) });
        }
    }
}
