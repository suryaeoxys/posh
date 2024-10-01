using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class ChnagedColumnNameDriverDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VehicleRegistrationPhoto",
                table: "TblDriverDocuments",
                newName: "VehicleRegistrationDocName");

            migrationBuilder.RenameColumn(
                name: "PassportPhoto",
                table: "TblDriverDocuments",
                newName: "PassportDocName");

            migrationBuilder.RenameColumn(
                name: "InsuarnceDocPhoto",
                table: "TblDriverDocuments",
                newName: "InsuarnceDocName");

            migrationBuilder.RenameColumn(
                name: "DrivingLicencePhoto",
                table: "TblDriverDocuments",
                newName: "DrivingLicenceDocName");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "2b8aaf0b-efe5-4c6b-8177-c3457d0dc213", new DateTime(2024, 1, 20, 14, 42, 44, 432, DateTimeKind.Local).AddTicks(2796), "AQAAAAEAACcQAAAAEIW9LA701/YwC/5m/3ZUNYs5+qMhYISoMLdJlrwMlX7KWafYeEL4qDD23dhTOVCE/w==", "e87108f4-8c39-407c-a04f-1845cd872c02", new DateTime(2024, 1, 20, 14, 42, 44, 432, DateTimeKind.Local).AddTicks(2805) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VehicleRegistrationDocName",
                table: "TblDriverDocuments",
                newName: "VehicleRegistrationPhoto");

            migrationBuilder.RenameColumn(
                name: "PassportDocName",
                table: "TblDriverDocuments",
                newName: "PassportPhoto");

            migrationBuilder.RenameColumn(
                name: "InsuarnceDocName",
                table: "TblDriverDocuments",
                newName: "InsuarnceDocPhoto");

            migrationBuilder.RenameColumn(
                name: "DrivingLicenceDocName",
                table: "TblDriverDocuments",
                newName: "DrivingLicencePhoto");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "57d3419c-adbb-42cf-be06-0f026063ac48", new DateTime(2024, 1, 19, 23, 50, 3, 410, DateTimeKind.Local).AddTicks(1663), "AQAAAAEAACcQAAAAEF9ZiIdLs8SjQDXQ0ZEWzP91IFNZ8HoAMo6q9secR+2Tof7i9KMOvSDcIcKs4tCFdA==", "a212d11d-3575-47de-b7cf-ad7b4932105c", new DateTime(2024, 1, 19, 23, 50, 3, 410, DateTimeKind.Local).AddTicks(1672) });
        }
    }
}
