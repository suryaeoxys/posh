using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedTblBookingStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblBookingStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblBookingStatus", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "StatusId", "UpdatedDate" },
                values: new object[] { "f692dfe9-ccb8-4d31-a842-eb782c994fa5", new DateTime(2024, 3, 11, 15, 19, 25, 275, DateTimeKind.Local).AddTicks(863), "AQAAAAEAACcQAAAAEBo3rCaL2lFWASCWxfw6ofhIdtOYuGFw2JEI4kWbWwzEDCBxix17fd5+eHwE9410AA==", "51d8cfc5-4762-49cc-9c6c-7ed4f8da753d", new Guid("57deeadb-b1c5-4273-a830-ed8d3b001f70"), new DateTime(2024, 3, 11, 15, 19, 25, 275, DateTimeKind.Local).AddTicks(876) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblBookingStatus");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "StatusId", "UpdatedDate" },
                values: new object[] { "8299b335-49bf-4428-8c60-5cad8dad1bf0", new DateTime(2024, 2, 28, 17, 15, 44, 551, DateTimeKind.Local).AddTicks(4972), "AQAAAAEAACcQAAAAEFXHlDNBusCPcixGAZqcse2Y8L7/HqqX5QTO25qrErnGU2o5G2u5RLr2TG8fFvmY5A==", "43d6fb24-ff1f-42e4-b5aa-0769be9f86f1", null, new DateTime(2024, 2, 28, 17, 15, 44, 551, DateTimeKind.Local).AddTicks(4985) });
        }
    }
}
