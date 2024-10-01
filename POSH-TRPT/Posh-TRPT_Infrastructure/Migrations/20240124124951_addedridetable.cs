using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class addedridetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblRideCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblRideCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TblCategoryPrices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BaseFare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cancel_Penalty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cost_Per_Mile = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cost_Per_Minute = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Maximum_Fare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Minimum_Fare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Sched_Cancel_Penalty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Sched_Ride_Minimum_Fare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Service_Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Toll_Fares = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Airport_Fees = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RideCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblCategoryPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TblCategoryPrices_Tbl_States_StateId",
                        column: x => x.StateId,
                        principalTable: "Tbl_States",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TblCategoryPrices_TblRideCategory_RideCategoryId",
                        column: x => x.RideCategoryId,
                        principalTable: "TblRideCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "4ccdc7f2-8ca2-48db-aa27-dd2641267138", new DateTime(2024, 1, 24, 18, 19, 50, 535, DateTimeKind.Local).AddTicks(8932), "AQAAAAEAACcQAAAAEEfzEzMFxYog0dyd7/0zXpY/RnKK1IBsQ6ztnlAmT529vOt9qShAYBOThnv2LsHLEg==", "e9b79827-5597-4565-90ce-26890b2b6b83", new DateTime(2024, 1, 24, 18, 19, 50, 535, DateTimeKind.Local).AddTicks(8941) });

            migrationBuilder.CreateIndex(
                name: "IX_TblCategoryPrices_RideCategoryId",
                table: "TblCategoryPrices",
                column: "RideCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TblCategoryPrices_StateId",
                table: "TblCategoryPrices",
                column: "StateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblCategoryPrices");

            migrationBuilder.DropTable(
                name: "TblRideCategory");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "828a33b3-bc38-471c-9437-d520d1e2f13d", new DateTime(2024, 1, 23, 19, 55, 56, 958, DateTimeKind.Local).AddTicks(4466), "AQAAAAEAACcQAAAAEDa94G3QipI0xsNDTclxtSWRuN8X1bFvGXF2/Xl3RFgsMknonHMKo41Ui/gb29mYkQ==", "ef5f6c53-af82-4273-aaed-d735e8620e57", new DateTime(2024, 1, 23, 19, 55, 56, 958, DateTimeKind.Local).AddTicks(4478) });
        }
    }
}
