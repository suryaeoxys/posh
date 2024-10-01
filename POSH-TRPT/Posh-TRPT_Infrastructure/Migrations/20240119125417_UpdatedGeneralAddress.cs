using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posh_TRPT_Infrastructure.Migrations
{
    public partial class UpdatedGeneralAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblGeneralAddress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Country = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    State = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    City = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                values: new object[] { "ebadb049-46bb-4f30-a46b-8d1c1b642d89", new DateTime(2024, 1, 19, 18, 24, 17, 568, DateTimeKind.Local).AddTicks(4074), "AQAAAAEAACcQAAAAED4vzxnjERekAi+PfhbSMfjTo++0rRza4FCuGupVsSYSakhftovL3Th4jYfH0RZLwQ==", "126d3c5e-1321-49c9-b9b7-1f2eadc6ae14", new DateTime(2024, 1, 19, 18, 24, 17, 568, DateTimeKind.Local).AddTicks(4087) });

            migrationBuilder.CreateIndex(
                name: "IX_TblGeneralAddress_UserId",
                table: "TblGeneralAddress",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblGeneralAddress");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "SecurityStamp", "UpdatedDate" },
                values: new object[] { "f7a9efdc-17ed-4681-b59e-271c40fb1dc7", new DateTime(2024, 1, 19, 18, 22, 15, 610, DateTimeKind.Local).AddTicks(3423), "AQAAAAEAACcQAAAAEOkqooVPsGGykQGYu9tU0fMIXlOVJXdzgBsipOjVn5lcuPuU/TA9kZcYXLaOfAwxug==", "75172397-917d-4981-b773-4a29b5653545", new DateTime(2024, 1, 19, 18, 22, 15, 610, DateTimeKind.Local).AddTicks(3432) });
        }
    }
}
