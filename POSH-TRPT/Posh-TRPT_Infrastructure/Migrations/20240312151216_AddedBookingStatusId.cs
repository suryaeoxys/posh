using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedBookingStatusId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BookingStatusId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("FB09BEF3-B1F2-4E21-A25D-A2D769315778"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "ac30d62f-8040-4bab-a3d3-565d8625df12", new DateTime(2024, 3, 12, 20, 42, 15, 829, DateTimeKind.Local).AddTicks(3288), "AQAAAAEAACcQAAAAEBBuVfn20pM6xuaois+HTTvWDfXy4iqEDk4KLBetRsrcw6Zim4XpBpP8P6rKphQStg==", "d7692436-5d68-4f35-80a7-c2e17e5415e2", new DateTime(2024, 3, 12, 20, 42, 15, 829, DateTimeKind.Local).AddTicks(3297) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingStatusId",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "42ff01df-5eb6-4d8e-9e5b-eee735c6cd99", new DateTime(2024, 3, 12, 20, 40, 54, 530, DateTimeKind.Local).AddTicks(3933), "AQAAAAEAACcQAAAAECegTSnWVHRVP7MCVpp+fNE7U/asdrzB+MfNS/0yXHMevCQBw1++C1MoOizPqZUi5A==", "0a49a323-b03d-4fab-ae8a-1a734709ce30", new DateTime(2024, 3, 12, 20, 40, 54, 530, DateTimeKind.Local).AddTicks(3948) });
        }
    }
}
