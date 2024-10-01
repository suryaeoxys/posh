using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class AddedLongitudeLatitudeASPNETUSER_db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "8299b335-49bf-4428-8c60-5cad8dad1bf0", new DateTime(2024, 2, 28, 17, 15, 44, 551, DateTimeKind.Local).AddTicks(4972), "AQAAAAEAACcQAAAAEFXHlDNBusCPcixGAZqcse2Y8L7/HqqX5QTO25qrErnGU2o5G2u5RLr2TG8fFvmY5A==", "43d6fb24-ff1f-42e4-b5aa-0769be9f86f1", new DateTime(2024, 2, 28, 17, 15, 44, 551, DateTimeKind.Local).AddTicks(4985) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "0f61468e-68bc-478f-aa8a-0e106b3853a0", new DateTime(2024, 2, 28, 16, 45, 16, 477, DateTimeKind.Local).AddTicks(7305), "AQAAAAEAACcQAAAAECjO12JjB4g9bS8y4VL8k2Stkqu5wjyMzV+mX2h0hYWt1Mg9/GeH9aLvb1I7G6+V5A==", "2813a77d-cb4a-44ad-93c6-7f0524ce23a9", new DateTime(2024, 2, 28, 16, 45, 16, 477, DateTimeKind.Local).AddTicks(7315) });
        }
    }
}
