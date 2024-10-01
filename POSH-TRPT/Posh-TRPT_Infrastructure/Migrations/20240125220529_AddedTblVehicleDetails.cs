using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedTblVehicleDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblVehicleDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Vehicle_Identification_Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Make = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vehicle_Plate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblVehicleDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TblVehicleDetails_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "2ee75b9d-bf7f-4adf-b335-52905d9fca44", new DateTime(2024, 1, 26, 3, 35, 27, 954, DateTimeKind.Local).AddTicks(979), "AQAAAAEAACcQAAAAELOdJFuX26Br6w2j1XQTP5+YoHzXOAJzWaNfCgttJ11Gf0CnbCM3CaQKzjKm8MQoRg==", "9298dfcd-8dd3-404a-b421-13fdd28ce9f1", new DateTime(2024, 1, 26, 3, 35, 27, 954, DateTimeKind.Local).AddTicks(991) });

            migrationBuilder.CreateIndex(
                name: "IX_TblVehicleDetails_UserId",
                table: "TblVehicleDetails",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblVehicleDetails");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "452e50a3-15bb-47f7-8754-16b9712b5daa", new DateTime(2024, 1, 26, 2, 48, 30, 686, DateTimeKind.Local).AddTicks(3552), "AQAAAAEAACcQAAAAEBw/dEcVcbzrbBgrN+qbxph3QaU/UmP6wWRm9kGVoWnwUcHFz1USy3ihZcbGVEPUCg==", "5f1b02f6-858a-426b-9025-7c73f4135bc6", new DateTime(2024, 1, 26, 2, 48, 30, 686, DateTimeKind.Local).AddTicks(3561) });
        }
    }
}
