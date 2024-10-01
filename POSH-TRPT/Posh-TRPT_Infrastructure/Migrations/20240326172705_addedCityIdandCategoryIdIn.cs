using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class addedCityIdandCategoryIdIn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "TblBookingDetail",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CityId",
                table: "TblBookingDetail",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "b5f263fa-23a3-4780-baea-cb1f14fe6196", new DateTime(2024, 3, 26, 22, 57, 5, 279, DateTimeKind.Local).AddTicks(6310), "AQAAAAEAACcQAAAAEGgqDNBC6rcapOIIKOlEjATpP0DGu52c6Fyzgy82yXsuxQUk15awWAqDshXEBKdbbw==", "e6d2b75f-665e-4db6-ab31-59eaf8d0f2cc", new DateTime(2024, 3, 26, 22, 57, 5, 279, DateTimeKind.Local).AddTicks(6324) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "TblBookingDetail");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "TblBookingDetail");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "f8bcf72b-b221-4bfe-8f47-73457a7b4747", new DateTime(2024, 3, 22, 21, 19, 51, 494, DateTimeKind.Local).AddTicks(9440), "AQAAAAEAACcQAAAAENLvAKOLYlyyz59yOkrBef7s9KcgT6X7185KrQrsK2pQvoo2iEO8ALw6fdjHbGQQ7g==", "46d2d81b-0d66-49c1-85ac-1ce39839c891", new DateTime(2024, 3, 22, 21, 19, 51, 494, DateTimeKind.Local).AddTicks(9449) });
        }
    }
}
