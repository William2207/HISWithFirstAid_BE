using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDepartmentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clinics_Departments_DepartmentId",
                table: "Clinics");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Departments_DepartmentId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Nurses_Departments_DepartmentId",
                table: "Nurses");

            migrationBuilder.DropForeignKey(
                name: "FK_Specialties_Departments_DepartmentId",
                table: "Specialties");

            migrationBuilder.DropForeignKey(
                name: "FK_Wards_Departments_DepartmentId",
                table: "Wards");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Specialties_DepartmentId",
                table: "Specialties");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_DepartmentId",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Clinics_DepartmentId",
                table: "Clinics");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Specialties");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Clinics");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Wards",
                newName: "SpecialityId");

            migrationBuilder.RenameIndex(
                name: "IX_Wards_DepartmentId",
                table: "Wards",
                newName: "IX_Wards_SpecialityId");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Nurses",
                newName: "SpecialityId");

            migrationBuilder.RenameIndex(
                name: "IX_Nurses_DepartmentId",
                table: "Nurses",
                newName: "IX_Nurses_SpecialityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Nurses_Specialties_SpecialityId",
                table: "Nurses",
                column: "SpecialityId",
                principalTable: "Specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wards_Specialties_SpecialityId",
                table: "Wards",
                column: "SpecialityId",
                principalTable: "Specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nurses_Specialties_SpecialityId",
                table: "Nurses");

            migrationBuilder.DropForeignKey(
                name: "FK_Wards_Specialties_SpecialityId",
                table: "Wards");

            migrationBuilder.RenameColumn(
                name: "SpecialityId",
                table: "Wards",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Wards_SpecialityId",
                table: "Wards",
                newName: "IX_Wards_DepartmentId");

            migrationBuilder.RenameColumn(
                name: "SpecialityId",
                table: "Nurses",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Nurses_SpecialityId",
                table: "Nurses",
                newName: "IX_Nurses_DepartmentId");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Specialties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Doctors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Clinics",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HeadDoctorId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Doctors_HeadDoctorId",
                        column: x => x.HeadDoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_DepartmentId",
                table: "Specialties",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_DepartmentId",
                table: "Doctors",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Clinics_DepartmentId",
                table: "Clinics",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_HeadDoctorId",
                table: "Departments",
                column: "HeadDoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clinics_Departments_DepartmentId",
                table: "Clinics",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Departments_DepartmentId",
                table: "Doctors",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Nurses_Departments_DepartmentId",
                table: "Nurses",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Specialties_Departments_DepartmentId",
                table: "Specialties",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Wards_Departments_DepartmentId",
                table: "Wards",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
