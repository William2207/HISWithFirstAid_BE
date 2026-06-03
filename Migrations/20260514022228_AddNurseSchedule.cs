using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddNurseSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NurseSchedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NurseId = table.Column<int>(type: "integer", nullable: false),
                    IsNightShift = table.Column<bool>(type: "boolean", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    IsOff = table.Column<bool>(type: "boolean", nullable: false),
                    WardId = table.Column<int>(type: "integer", nullable: true),
                    SpecialtyId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NurseSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NurseSchedule_Nurses_NurseId",
                        column: x => x.NurseId,
                        principalTable: "Nurses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NurseSchedule_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NurseSchedule_Wards_WardId",
                        column: x => x.WardId,
                        principalTable: "Wards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NurseSchedule_NurseId",
                table: "NurseSchedule",
                column: "NurseId");

            migrationBuilder.CreateIndex(
                name: "IX_NurseSchedule_SpecialtyId",
                table: "NurseSchedule",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_NurseSchedule_WardId",
                table: "NurseSchedule",
                column: "WardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NurseSchedule");
        }
    }
}
