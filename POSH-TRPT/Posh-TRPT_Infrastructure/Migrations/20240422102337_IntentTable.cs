using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class IntentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblStripeCustomersPaymentIntent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerPaymentIntentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StripeCustomerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount_Received = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Canceled_At = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cancellation_Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Client_Secret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latest_Charge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Livemode = table.Column<bool>(type: "bit", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Customer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Payment_Method = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblStripeCustomersPaymentIntent", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "7f158fb7-eb22-4f05-b670-f506be4a0a7b", new DateTime(2024, 4, 22, 15, 53, 37, 612, DateTimeKind.Local).AddTicks(2846), "AQAAAAEAACcQAAAAEIVO+u+9VyITrY2NPXGogDWD6sD7WEdbKV6TFt3ImsxW3QyvpfD5b2R1iorabhYz6w==", "5454c183-ed4d-4583-95b5-8cf34c2933d9", new DateTime(2024, 4, 22, 15, 53, 37, 612, DateTimeKind.Local).AddTicks(2857) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblStripeCustomersPaymentIntent");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "01dd5e5a-c15b-4263-b05e-54afcdb6e9e1", new DateTime(2024, 4, 19, 16, 8, 45, 881, DateTimeKind.Local).AddTicks(1533), "AQAAAAEAACcQAAAAEEhC3PV5ewst60XZvyhFzwVNaiDWe7jL1Cg3cet+/dIUJ9XA5ojk4vvqu/5FUAtkew==", "4dad4768-12f2-4ec2-b32e-d8753df143bd", new DateTime(2024, 4, 19, 16, 8, 45, 881, DateTimeKind.Local).AddTicks(1543) });
        }
    }
}
