using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class ModifyMedicalRecordFKVitalSign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VitalSigns_MedicalRecordId",
                table: "VitalSigns");

            migrationBuilder.CreateIndex(
                name: "IX_VitalSigns_MedicalRecordId",
                table: "VitalSigns",
                column: "MedicalRecordId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VitalSigns_MedicalRecordId",
                table: "VitalSigns");

            migrationBuilder.CreateIndex(
                name: "IX_VitalSigns_MedicalRecordId",
                table: "VitalSigns",
                column: "MedicalRecordId");
        }
    }
}
