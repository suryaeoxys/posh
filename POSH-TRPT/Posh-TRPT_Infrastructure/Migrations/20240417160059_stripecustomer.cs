using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class stripecustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblStripeCustomers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StripeCustomerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Livemode = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblStripeCustomers", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "2639de4e-5227-4ffb-80a5-ecc8d75334ee", new DateTime(2024, 4, 17, 21, 30, 58, 830, DateTimeKind.Local).AddTicks(8297), "AQAAAAEAACcQAAAAEAtM8wmzejseZ2aAuK+7ATh+YBsRmY3CuZzgZ3QfhVbOvY4L5ZVrDpRJMXstXYouwg==", "8bf71658-fe73-49dd-9713-bc00265790e2", new DateTime(2024, 4, 17, 21, 30, 58, 830, DateTimeKind.Local).AddTicks(8309) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblStripeCustomers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "d01b7c69-c5af-419a-83cc-413baf00a786", new DateTime(2024, 4, 15, 17, 22, 37, 94, DateTimeKind.Local).AddTicks(8017), "AQAAAAEAACcQAAAAEHFkcCq0virebFybEX094T9zi/8qROyCRMArFyvCbJZbH9BxSrLJDhtAgvkZMCfOGw==", "ab6c59be-8ff1-46c9-b64d-ff64cc28f994", new DateTime(2024, 4, 15, 17, 22, 37, 94, DateTimeKind.Local).AddTicks(8026) });
        }
    }
}
