using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class ShiftTypeModifyDoctorSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "DoctorSchedule");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "DoctorSchedule");

            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "DoctorSchedule",
                newName: "IsOff");

            migrationBuilder.RenameColumn(
                name: "DayOfWeek",
                table: "DoctorSchedule",
                newName: "ShiftTypeId");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "DoctorSchedule",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "SpecialtyId",
                table: "DoctorSchedule",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShiftTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    IsNightShift = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSchedule_ShiftTypeId",
                table: "DoctorSchedule",
                column: "ShiftTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSchedule_SpecialtyId",
                table: "DoctorSchedule",
                column: "SpecialtyId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSchedule_ShiftTypes_ShiftTypeId",
                table: "DoctorSchedule",
                column: "ShiftTypeId",
                principalTable: "ShiftTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSchedule_Specialties_SpecialtyId",
                table: "DoctorSchedule",
                column: "SpecialtyId",
                principalTable: "Specialties",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSchedule_ShiftTypes_ShiftTypeId",
                table: "DoctorSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSchedule_Specialties_SpecialtyId",
                table: "DoctorSchedule");

            migrationBuilder.DropTable(
                name: "ShiftTypes");

            migrationBuilder.DropIndex(
                name: "IX_DoctorSchedule_ShiftTypeId",
                table: "DoctorSchedule");

            migrationBuilder.DropIndex(
                name: "IX_DoctorSchedule_SpecialtyId",
                table: "DoctorSchedule");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "DoctorSchedule");

            migrationBuilder.DropColumn(
                name: "SpecialtyId",
                table: "DoctorSchedule");

            migrationBuilder.RenameColumn(
                name: "ShiftTypeId",
                table: "DoctorSchedule",
                newName: "DayOfWeek");

            migrationBuilder.RenameColumn(
                name: "IsOff",
                table: "DoctorSchedule",
                newName: "IsAvailable");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "DoctorSchedule",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "DoctorSchedule",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
