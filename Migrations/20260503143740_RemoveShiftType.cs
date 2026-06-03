using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveShiftType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSchedule_ShiftTypes_ShiftTypeId",
                table: "DoctorSchedule");

            migrationBuilder.DropTable(
                name: "ShiftTypes");

            migrationBuilder.DropIndex(
                name: "IX_DoctorSchedule_ShiftTypeId",
                table: "DoctorSchedule");

            migrationBuilder.DropColumn(
                name: "ShiftTypeId",
                table: "DoctorSchedule");

            migrationBuilder.AddColumn<bool>(
                name: "IsNightShift",
                table: "DoctorSchedule",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNightShift",
                table: "DoctorSchedule");

            migrationBuilder.AddColumn<int>(
                name: "ShiftTypeId",
                table: "DoctorSchedule",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShiftTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsNightShift = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSchedule_ShiftTypeId",
                table: "DoctorSchedule",
                column: "ShiftTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSchedule_ShiftTypes_ShiftTypeId",
                table: "DoctorSchedule",
                column: "ShiftTypeId",
                principalTable: "ShiftTypes",
                principalColumn: "Id");
        }
    }
}
