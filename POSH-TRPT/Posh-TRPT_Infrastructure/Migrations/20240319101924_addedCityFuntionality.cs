using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class addedCityFuntionality : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CityId",
                table: "TblCategoryPrices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "4f4eff22-0319-4929-ba91-7dd236f82ac2", new DateTime(2024, 3, 19, 15, 49, 23, 898, DateTimeKind.Local).AddTicks(3308), "AQAAAAEAACcQAAAAEGcJb1IzYtw/cshlGJkoKAJVeQhde4fnbgloZO1LW0T5oW9H0bge2XyulGrMRvon3w==", "7e748d7a-0c37-4973-bd71-5d9f27552907", new DateTime(2024, 3, 19, 15, 49, 23, 898, DateTimeKind.Local).AddTicks(3316) });

            migrationBuilder.CreateIndex(
                name: "IX_TblCategoryPrices_CityId",
                table: "TblCategoryPrices",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_TblCategoryPrices_Tbl_Cities_CityId",
                table: "TblCategoryPrices",
                column: "CityId",
                principalTable: "Tbl_Cities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblCategoryPrices_Tbl_Cities_CityId",
                table: "TblCategoryPrices");

            migrationBuilder.DropIndex(
                name: "IX_TblCategoryPrices_CityId",
                table: "TblCategoryPrices");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "TblCategoryPrices");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "6bca91a5-e3e9-4cf0-a919-4e34d54348f0", new DateTime(2024, 3, 18, 20, 42, 3, 50, DateTimeKind.Local).AddTicks(477), "AQAAAAEAACcQAAAAENPOHRWj0SlTxN12ocSc2viib3i8phfL7suD8IAJ+YmSOuQqHHgH6Gjoh9z/dAgmBA==", "0a0600af-3247-4191-a8fb-3b36380980c6", new DateTime(2024, 3, 18, 20, 42, 3, 50, DateTimeKind.Local).AddTicks(484) });
        }
    }
}
