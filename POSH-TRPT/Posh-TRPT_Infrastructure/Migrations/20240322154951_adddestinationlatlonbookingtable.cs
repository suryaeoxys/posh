using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class adddestinationlatlonbookingtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "RiderDestinationLat",
                table: "TblBookingDetail",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RiderDestinationLong",
                table: "TblBookingDetail",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "f8bcf72b-b221-4bfe-8f47-73457a7b4747", new DateTime(2024, 3, 22, 21, 19, 51, 494, DateTimeKind.Local).AddTicks(9440), "AQAAAAEAACcQAAAAENLvAKOLYlyyz59yOkrBef7s9KcgT6X7185KrQrsK2pQvoo2iEO8ALw6fdjHbGQQ7g==", "46d2d81b-0d66-49c1-85ac-1ce39839c891", new DateTime(2024, 3, 22, 21, 19, 51, 494, DateTimeKind.Local).AddTicks(9449) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RiderDestinationLat",
                table: "TblBookingDetail");

            migrationBuilder.DropColumn(
                name: "RiderDestinationLong",
                table: "TblBookingDetail");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "5590a96c-93fd-4b31-a501-5d2116b808f5", new DateTime(2024, 3, 22, 21, 6, 58, 870, DateTimeKind.Local).AddTicks(7428), "AQAAAAEAACcQAAAAEIeZCjrjJzDQqDf1ljaYzJXXTSIvY/SKGAV4LiK0XoCCH4sUfZeC20cyMZpAG7teLg==", "9a9e1bf7-f2a5-4b35-9b13-396673b8a8dc", new DateTime(2024, 3, 22, 21, 6, 58, 870, DateTimeKind.Local).AddTicks(7436) });
        }
    }
}
