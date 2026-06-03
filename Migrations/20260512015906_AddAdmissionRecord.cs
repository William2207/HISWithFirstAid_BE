using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAdmissionRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdmissionRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    BedId = table.Column<int>(type: "integer", nullable: false),
                    MedicalRecordId = table.Column<int>(type: "integer", nullable: false),
                    AdmittedByNurseId = table.Column<int>(type: "integer", nullable: false),
                    AdmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    DischargedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdmissionRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdmissionRecords_Beds_BedId",
                        column: x => x.BedId,
                        principalTable: "Beds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdmissionRecords_MedicalRecords_MedicalRecordId",
                        column: x => x.MedicalRecordId,
                        principalTable: "MedicalRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdmissionRecords_Nurses_AdmittedByNurseId",
                        column: x => x.AdmittedByNurseId,
                        principalTable: "Nurses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdmissionRecords_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionRecords_AdmittedByNurseId",
                table: "AdmissionRecords",
                column: "AdmittedByNurseId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionRecords_BedId",
                table: "AdmissionRecords",
                column: "BedId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionRecords_MedicalRecordId",
                table: "AdmissionRecords",
                column: "MedicalRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionRecords_PatientId_DischargedAt",
                table: "AdmissionRecords",
                columns: new[] { "PatientId", "DischargedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdmissionRecords");
        }
    }
}
