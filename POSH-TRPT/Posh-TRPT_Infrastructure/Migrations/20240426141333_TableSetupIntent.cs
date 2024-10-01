using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class TableSetupIntent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblStripeCustomersSetupIntent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StripeCustomerRecordId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Livemode = table.Column<bool>(type: "bit", nullable: false),
                    EphemeralKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerPaymentIntentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Client_Secret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EphemeralKey_Secret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latest_Charge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Payment_Method = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Usage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Payment_Method_Types = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblStripeCustomersSetupIntent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TblStripeCustomersSetupIntent_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TblStripeCustomersSetupIntent_TblStripeCustomers_StripeCustomerRecordId",
                        column: x => x.StripeCustomerRecordId,
                        principalTable: "TblStripeCustomers",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "74c209eb-105d-4935-9928-f0ba4c4231bb", new DateTime(2024, 4, 26, 19, 43, 33, 410, DateTimeKind.Local).AddTicks(5246), "AQAAAAEAACcQAAAAEBW42wlN56qUaSKKst4Z/V4t6KNWXvbb6RtW+EtZjpOyybMtUnRprdT1ZSTZpfy4cA==", "926909d3-62bb-4b43-a53c-85d64f262cfd", new DateTime(2024, 4, 26, 19, 43, 33, 410, DateTimeKind.Local).AddTicks(5256) });

            migrationBuilder.CreateIndex(
                name: "IX_TblStripeCustomersSetupIntent_StripeCustomerRecordId",
                table: "TblStripeCustomersSetupIntent",
                column: "StripeCustomerRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_TblStripeCustomersSetupIntent_UserId",
                table: "TblStripeCustomersSetupIntent",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblStripeCustomersSetupIntent");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "c5287c8a-aed7-4d0d-bd6f-f13026419d86", new DateTime(2024, 4, 25, 2, 0, 34, 556, DateTimeKind.Local).AddTicks(6839), "AQAAAAEAACcQAAAAECoLwcWV82///3eoi9CrA0gJrDb3lH2X3GvCC4DACZgyGZ2BXoglFzawCHPYD5us6Q==", "ac5963cb-3d63-4656-828b-7c2f12e16d37", new DateTime(2024, 4, 25, 2, 0, 34, 556, DateTimeKind.Local).AddTicks(6848) });
        }
    }
}
