using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class addedStatusTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblVehicleDetails_AspNetUsers_UserId",
                table: "TblVehicleDetails");

            migrationBuilder.AlterColumn<string>(
                name: "Year",
                table: "TblVehicleDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Vehicle_Plate",
                table: "TblVehicleDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Vehicle_Identification_Number",
                table: "TblVehicleDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TblVehicleDetails",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "TblVehicleDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Make",
                table: "TblVehicleDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "TblVehicleDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "StatusId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TblStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblStatus", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "8a9f3411-a519-4ebc-9f67-490482f8cad1", new DateTime(2024, 2, 1, 14, 50, 53, 728, DateTimeKind.Local).AddTicks(8785), "AQAAAAEAACcQAAAAEKTaIQbZ6y4P7evwQdM9tPKeonnJgwD8SC7HKLBtFfyWRQOe03V6r99wImzdljDe2Q==", "988c283f-26cd-4723-9115-06d6713cfe9c", new DateTime(2024, 2, 1, 14, 50, 53, 728, DateTimeKind.Local).AddTicks(8794) });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_StatusId",
                table: "AspNetUsers",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_TblStatus_StatusId",
                table: "AspNetUsers",
                column: "StatusId",
                principalTable: "TblStatus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TblVehicleDetails_AspNetUsers_UserId",
                table: "TblVehicleDetails",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_TblStatus_StatusId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_TblVehicleDetails_AspNetUsers_UserId",
                table: "TblVehicleDetails");

            migrationBuilder.DropTable(
                name: "TblStatus");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_StatusId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Year",
                table: "TblVehicleDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Vehicle_Plate",
                table: "TblVehicleDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Vehicle_Identification_Number",
                table: "TblVehicleDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TblVehicleDetails",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "TblVehicleDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Make",
                table: "TblVehicleDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "TblVehicleDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "67c852a4-72d6-4f84-b486-dab876d3563f", new DateTime(2024, 1, 29, 23, 53, 33, 759, DateTimeKind.Local).AddTicks(7673), "AQAAAAEAACcQAAAAEITH98dZMjjzBdhBByfLkz/K5sy9Z3F5VXQxm/5ww/KuNXmv1umXUiZiyX3efnLqUw==", "c7b8fc95-fd3f-4ae9-9ddf-356d042fe41c", new DateTime(2024, 1, 29, 23, 53, 33, 759, DateTimeKind.Local).AddTicks(7686) });

            migrationBuilder.AddForeignKey(
                name: "FK_TblVehicleDetails_AspNetUsers_UserId",
                table: "TblVehicleDetails",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
