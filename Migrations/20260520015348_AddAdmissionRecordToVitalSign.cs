using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAdmissionRecordToVitalSign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VitalSigns_MedicalRecords_MedicalRecordId",
                table: "VitalSigns");

            migrationBuilder.AlterColumn<int>(
                name: "MedicalRecordId",
                table: "VitalSigns",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "AdmissionRecordId",
                table: "VitalSigns",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VitalSigns_AdmissionRecordId",
                table: "VitalSigns",
                column: "AdmissionRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_VitalSigns_AdmissionRecords_AdmissionRecordId",
                table: "VitalSigns",
                column: "AdmissionRecordId",
                principalTable: "AdmissionRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VitalSigns_MedicalRecords_MedicalRecordId",
                table: "VitalSigns",
                column: "MedicalRecordId",
                principalTable: "MedicalRecords",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VitalSigns_AdmissionRecords_AdmissionRecordId",
                table: "VitalSigns");

            migrationBuilder.DropForeignKey(
                name: "FK_VitalSigns_MedicalRecords_MedicalRecordId",
                table: "VitalSigns");

            migrationBuilder.DropIndex(
                name: "IX_VitalSigns_AdmissionRecordId",
                table: "VitalSigns");

            migrationBuilder.DropColumn(
                name: "AdmissionRecordId",
                table: "VitalSigns");

            migrationBuilder.AlterColumn<int>(
                name: "MedicalRecordId",
                table: "VitalSigns",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VitalSigns_MedicalRecords_MedicalRecordId",
                table: "VitalSigns",
                column: "MedicalRecordId",
                principalTable: "MedicalRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
