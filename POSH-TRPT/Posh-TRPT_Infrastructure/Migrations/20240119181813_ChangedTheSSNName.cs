using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class ChangedTheSSNName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Social_Security_Numbar",
                table: "TblDriverDocuments",
                newName: "Social_Security_Number");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "a082021d-da56-4b69-a62f-282899fd7000", new DateTime(2024, 1, 19, 23, 48, 13, 74, DateTimeKind.Local).AddTicks(5960), "AQAAAAEAACcQAAAAEAgKIi6z8zIxJCNFqIXfimUYSku8/qfNbqEZzkMXQOa16Y1k+Q5ZGIfycwyBll44Ig==", "3763b8be-7041-46ad-be95-faec57674e5e", new DateTime(2024, 1, 19, 23, 48, 13, 74, DateTimeKind.Local).AddTicks(5968) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Social_Security_Number",
                table: "TblDriverDocuments",
                newName: "Social_Security_Numbar");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "731a29ca-303e-4960-8a94-b28039b9e49a", new DateTime(2024, 1, 19, 22, 3, 14, 452, DateTimeKind.Local).AddTicks(499), "AQAAAAEAACcQAAAAEBI1nuDA35JFwcEne8MNKBhfIniVkToHLp9U8U+G5cKfDAA3Ts8nt7gLgvPf4WB+cA==", "fceeca4a-ace2-4807-aff4-8090caa2622e", new DateTime(2024, 1, 19, 22, 3, 14, 452, DateTimeKind.Local).AddTicks(507) });
        }
    }
}
