using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedBookingDetailtableNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblBookingDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DriverUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RiderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiderUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CategoryPriceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RiderSourceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RiderLat = table.Column<double>(type: "float", nullable: true),
                    RiderLong = table.Column<double>(type: "float", nullable: true),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Distance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DestinationPlaceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblBookingDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TblBookingDetail_AspNetUsers_DriverUserId",
                        column: x => x.DriverUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TblBookingDetail_AspNetUsers_RiderUserId",
                        column: x => x.RiderUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TblBookingDetail_TblCategoryPrices_CategoryPriceId",
                        column: x => x.CategoryPriceId,
                        principalTable: "TblCategoryPrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TblBookingDetail_TblVehicleDetails_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "TblVehicleDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "38528130-3979-4166-980a-6d7056af2531", new DateTime(2024, 3, 22, 20, 56, 2, 189, DateTimeKind.Local).AddTicks(1097), "AQAAAAEAACcQAAAAEHFigoL6nticCYzP/7xz/qqEIJ8i5MqD5/g11xWfxj2Lu/XgHOUIeOL5g1h+aUW3Mw==", "465e8d54-84a9-4cef-bfbd-41df192d9947", new DateTime(2024, 3, 22, 20, 56, 2, 189, DateTimeKind.Local).AddTicks(1111) });

            migrationBuilder.CreateIndex(
                name: "IX_TblBookingDetail_CategoryPriceId",
                table: "TblBookingDetail",
                column: "CategoryPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_TblBookingDetail_DriverUserId",
                table: "TblBookingDetail",
                column: "DriverUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TblBookingDetail_RiderUserId",
                table: "TblBookingDetail",
                column: "RiderUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TblBookingDetail_VehicleId",
                table: "TblBookingDetail",
                column: "VehicleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblBookingDetail");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "dc4170a4-77c0-4623-b5ee-d2547b772ce1", new DateTime(2024, 3, 22, 20, 50, 54, 85, DateTimeKind.Local).AddTicks(906), "AQAAAAEAACcQAAAAEAYwRtkZ97xkUiQ/OkLPiqGl8RFtyp8K1PyPvtEiwIDQtXjdqyrm+f5xJLolWBIq8g==", "a975d4bc-83eb-4069-bfda-cdbcbf11d4d5", new DateTime(2024, 3, 22, 20, 50, 54, 85, DateTimeKind.Local).AddTicks(914) });
        }
    }
}
