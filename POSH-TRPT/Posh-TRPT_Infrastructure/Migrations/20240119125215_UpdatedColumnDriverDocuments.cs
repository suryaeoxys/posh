using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class UpdatedColumnDriverDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblGeneralAddress");

            migrationBuilder.RenameColumn(
                name: "IsBackgroundChecked",
                table: "AspNetUsers",
                newName: "IsTwilioVerified");

            migrationBuilder.AddColumn<bool>(
                name: "IsBackgroundVerificationChecked",
                table: "TblDriverDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "f7a9efdc-17ed-4681-b59e-271c40fb1dc7", new DateTime(2024, 1, 19, 18, 22, 15, 610, DateTimeKind.Local).AddTicks(3423), "AQAAAAEAACcQAAAAEOkqooVPsGGykQGYu9tU0fMIXlOVJXdzgBsipOjVn5lcuPuU/TA9kZcYXLaOfAwxug==", "75172397-917d-4981-b773-4a29b5653545", new DateTime(2024, 1, 19, 18, 22, 15, 610, DateTimeKind.Local).AddTicks(3432) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBackgroundVerificationChecked",
                table: "TblDriverDocuments");

            migrationBuilder.RenameColumn(
                name: "IsTwilioVerified",
                table: "AspNetUsers",
                newName: "IsBackgroundChecked");

            migrationBuilder.CreateTable(
                name: "TblGeneralAddress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblGeneralAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TblGeneralAddress_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "395d0a83-ecfc-421b-b0e8-9e17e6628523", new DateTime(2024, 1, 17, 20, 39, 47, 502, DateTimeKind.Local).AddTicks(4392), "AQAAAAEAACcQAAAAEJdgKUCu8E4+vB0BPAaOESolqa6+MKfBnMRak7hJwSDsfXPrbjbJo8pcmmlxHi6csg==", "a1d06178-2f81-460f-bb3e-e1e30d1dddb1", new DateTime(2024, 1, 17, 20, 39, 47, 502, DateTimeKind.Local).AddTicks(4406) });

            migrationBuilder.CreateIndex(
                name: "IX_TblGeneralAddress_UserId",
                table: "TblGeneralAddress",
                column: "UserId");
        }
    }
}
