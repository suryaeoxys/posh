using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class nullableFKBookingStatusId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "BookingStatusId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "BookingStatusId", "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { null, "20aba648-75ce-44a9-9f04-e814a3ad56b3", new DateTime(2024, 3, 12, 16, 2, 7, 637, DateTimeKind.Local).AddTicks(3763), "AQAAAAEAACcQAAAAEJgtVf/ASstCusbCgtEvInyaZgtyO1zqueqrDwHOnhn0KC06ws1DbAJ54mVHFO53fw==", "b880d316-d8cd-400f-b66b-f0e6e1d245b3", new DateTime(2024, 3, 12, 16, 2, 7, 637, DateTimeKind.Local).AddTicks(3776) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "BookingStatusId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "BookingStatusId", "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000000"), "801a75d8-756a-496e-9a83-5af12a80b764", new DateTime(2024, 3, 12, 16, 0, 36, 440, DateTimeKind.Local).AddTicks(9223), "AQAAAAEAACcQAAAAEP1N1SkRs2GmJlJp7Bx/7/pLPt6I1gl06HYBvzmFqDN+iiA8keVcq00jYxU5sPWhCQ==", "28c41cbb-f33a-416f-96d0-05598daf59b9", new DateTime(2024, 3, 12, 16, 0, 36, 440, DateTimeKind.Local).AddTicks(9232) });
        }
    }
}
