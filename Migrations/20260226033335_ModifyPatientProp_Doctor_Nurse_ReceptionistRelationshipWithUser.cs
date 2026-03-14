using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class ModifyPatientProp_Doctor_Nurse_ReceptionistRelationshipWithUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_PatientProfile_PatientProfileId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Users_DoctorId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Bed_Users_CurrentPatientId",
                table: "Bed");

            migrationBuilder.DropForeignKey(
                name: "FK_Department_Users_HeadDoctorId",
                table: "Department");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSchedule_Users_DoctorId",
                table: "DoctorSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpecialty_Users_DoctorId",
                table: "DoctorSpecialty");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_PatientProfile_PatientProfileId",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecord_PatientProfile_PatientProfileId",
                table: "MedicalRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecord_Users_DoctorId",
                table: "MedicalRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_PatientProfile_PatientProfileId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Users_CashierId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Queue_PatientProfile_PatientProfileId",
                table: "Queue");

            migrationBuilder.DropForeignKey(
                name: "FK_Queue_Users_ReceptionistId",
                table: "Queue");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Department_DepartmentId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Department_Doctor_DepartmentId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Specialty_PrimarySpecialtyId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_VitalSign_Users_NurseId",
                table: "VitalSign");

            migrationBuilder.DropTable(
                name: "PatientProfile");

            migrationBuilder.DropIndex(
                name: "IX_Users_DepartmentId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Doctor_DepartmentId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PrimarySpecialtyId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_MedicalRecord_PatientProfileId",
                table: "MedicalRecord");

            migrationBuilder.DropColumn(
                name: "Allergies",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BloodType",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ConsultationFee",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Doctor_DepartmentId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Doctor_IsAvailable",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Doctor_LicenseNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Doctor_Qualifications",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Doctor_YearsOfExperience",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmergencyContact",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmergencyContactName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LicenseNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Nurse_IsAvailable",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PrimarySpecialtyId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Qualifications",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WorkStation",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "YearsOfExperience",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PatientProfileId",
                table: "MedicalRecord");

            migrationBuilder.RenameColumn(
                name: "PatientProfileId",
                table: "Queue",
                newName: "PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Queue_PatientProfileId",
                table: "Queue",
                newName: "IX_Queue_PatientId");

            migrationBuilder.RenameColumn(
                name: "PatientProfileId",
                table: "Payment",
                newName: "PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_PatientProfileId",
                table: "Payment",
                newName: "IX_Payment_PatientId");

            migrationBuilder.RenameColumn(
                name: "PatientProfileId",
                table: "Invoice",
                newName: "PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoice_PatientProfileId",
                table: "Invoice",
                newName: "IX_Invoice_PatientId");

            migrationBuilder.RenameColumn(
                name: "PatientProfileId",
                table: "Appointment",
                newName: "PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_PatientProfileId",
                table: "Appointment",
                newName: "IX_Appointment_PatientId");

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "MedicalRecord",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Doctor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    PrimarySpecialtyId = table.Column<int>(type: "integer", nullable: false),
                    LicenseNumber = table.Column<string>(type: "text", nullable: false),
                    Qualifications = table.Column<string>(type: "text", nullable: true),
                    YearsOfExperience = table.Column<int>(type: "integer", nullable: false),
                    ConsultationFee = table.Column<decimal>(type: "numeric", nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doctor_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Doctor_Specialty_PrimarySpecialtyId",
                        column: x => x.PrimarySpecialtyId,
                        principalTable: "Specialty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Doctor_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Nurse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    LicenseNumber = table.Column<string>(type: "text", nullable: false),
                    Qualifications = table.Column<string>(type: "text", nullable: true),
                    YearsOfExperience = table.Column<int>(type: "integer", nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nurse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nurse_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Nurse_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Receptionist",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    WorkStation = table.Column<string>(type: "text", nullable: true),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receptionist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Receptionist_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Gender = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    IdCard = table.Column<string>(type: "text", nullable: true),
                    InsuranceNumber = table.Column<string>(type: "text", nullable: true),
                    BloodType = table.Column<string>(type: "text", nullable: true),
                    Allergies = table.Column<string>(type: "text", nullable: true),
                    MedicalHistory = table.Column<string>(type: "text", nullable: true),
                    EmergencyContactName = table.Column<string>(type: "text", nullable: true),
                    EmergencyContact = table.Column<string>(type: "text", nullable: true),
                    EmergencyContactRelationship = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatorId = table.Column<int>(type: "integer", nullable: false),
                    ReceptionistId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patient_Receptionist_ReceptionistId",
                        column: x => x.ReceptionistId,
                        principalTable: "Receptionist",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Patient_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patient_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecord_PatientId",
                table: "MedicalRecord",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_DepartmentId",
                table: "Doctor",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_PrimarySpecialtyId",
                table: "Doctor",
                column: "PrimarySpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_UserId",
                table: "Doctor",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nurse_DepartmentId",
                table: "Nurse",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Nurse_UserId",
                table: "Nurse",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patient_CreatorId",
                table: "Patient",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_ReceptionistId",
                table: "Patient",
                column: "ReceptionistId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_UserId",
                table: "Patient",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Receptionist_UserId",
                table: "Receptionist",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Doctor_DoctorId",
                table: "Appointment",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Patient_PatientId",
                table: "Appointment",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bed_Patient_CurrentPatientId",
                table: "Bed",
                column: "CurrentPatientId",
                principalTable: "Patient",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Department_Doctor_HeadDoctorId",
                table: "Department",
                column: "HeadDoctorId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSchedule_Doctor_DoctorId",
                table: "DoctorSchedule",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpecialty_Doctor_DoctorId",
                table: "DoctorSpecialty",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Patient_PatientId",
                table: "Invoice",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecord_Doctor_DoctorId",
                table: "MedicalRecord",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecord_Patient_PatientId",
                table: "MedicalRecord",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Patient_PatientId",
                table: "Payment",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Receptionist_CashierId",
                table: "Payment",
                column: "CashierId",
                principalTable: "Receptionist",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Queue_Patient_PatientId",
                table: "Queue",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Queue_Receptionist_ReceptionistId",
                table: "Queue",
                column: "ReceptionistId",
                principalTable: "Receptionist",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VitalSign_Nurse_NurseId",
                table: "VitalSign",
                column: "NurseId",
                principalTable: "Nurse",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Doctor_DoctorId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Patient_PatientId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Bed_Patient_CurrentPatientId",
                table: "Bed");

            migrationBuilder.DropForeignKey(
                name: "FK_Department_Doctor_HeadDoctorId",
                table: "Department");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSchedule_Doctor_DoctorId",
                table: "DoctorSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpecialty_Doctor_DoctorId",
                table: "DoctorSpecialty");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Patient_PatientId",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecord_Doctor_DoctorId",
                table: "MedicalRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecord_Patient_PatientId",
                table: "MedicalRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Patient_PatientId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Receptionist_CashierId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Queue_Patient_PatientId",
                table: "Queue");

            migrationBuilder.DropForeignKey(
                name: "FK_Queue_Receptionist_ReceptionistId",
                table: "Queue");

            migrationBuilder.DropForeignKey(
                name: "FK_VitalSign_Nurse_NurseId",
                table: "VitalSign");

            migrationBuilder.DropTable(
                name: "Doctor");

            migrationBuilder.DropTable(
                name: "Nurse");

            migrationBuilder.DropTable(
                name: "Patient");

            migrationBuilder.DropTable(
                name: "Receptionist");

            migrationBuilder.DropIndex(
                name: "IX_MedicalRecord_PatientId",
                table: "MedicalRecord");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "MedicalRecord");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "Queue",
                newName: "PatientProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Queue_PatientId",
                table: "Queue",
                newName: "IX_Queue_PatientProfileId");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "Payment",
                newName: "PatientProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_PatientId",
                table: "Payment",
                newName: "IX_Payment_PatientProfileId");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "Invoice",
                newName: "PatientProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoice_PatientId",
                table: "Invoice",
                newName: "IX_Invoice_PatientProfileId");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "Appointment",
                newName: "PatientProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_PatientId",
                table: "Appointment",
                newName: "IX_Appointment_PatientProfileId");

            migrationBuilder.AddColumn<string>(
                name: "Allergies",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BloodType",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ConsultationFee",
                table: "Users",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Doctor_DepartmentId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Doctor_IsAvailable",
                table: "Users",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Doctor_LicenseNumber",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Doctor_Qualifications",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Doctor_YearsOfExperience",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContact",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactName",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Users",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicenseNumber",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Nurse_IsAvailable",
                table: "Users",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PrimarySpecialtyId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Qualifications",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserType",
                table: "Users",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WorkStation",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YearsOfExperience",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PatientProfileId",
                table: "MedicalRecord",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PatientProfile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatorId = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Allergies = table.Column<string>(type: "text", nullable: true),
                    BloodType = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EmergencyContact = table.Column<string>(type: "text", nullable: true),
                    EmergencyContactName = table.Column<string>(type: "text", nullable: true),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    IdCard = table.Column<string>(type: "text", nullable: true),
                    InsuranceNumber = table.Column<string>(type: "text", nullable: true),
                    MedicalHistory = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    ReceptionistId = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientProfile_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientProfile_Users_ReceptionistId",
                        column: x => x.ReceptionistId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientProfile_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentId",
                table: "Users",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Doctor_DepartmentId",
                table: "Users",
                column: "Doctor_DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PrimarySpecialtyId",
                table: "Users",
                column: "PrimarySpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecord_PatientProfileId",
                table: "MedicalRecord",
                column: "PatientProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientProfile_CreatorId",
                table: "PatientProfile",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientProfile_ReceptionistId",
                table: "PatientProfile",
                column: "ReceptionistId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientProfile_UserId",
                table: "PatientProfile",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_PatientProfile_PatientProfileId",
                table: "Appointment",
                column: "PatientProfileId",
                principalTable: "PatientProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Users_DoctorId",
                table: "Appointment",
                column: "DoctorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bed_Users_CurrentPatientId",
                table: "Bed",
                column: "CurrentPatientId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Department_Users_HeadDoctorId",
                table: "Department",
                column: "HeadDoctorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSchedule_Users_DoctorId",
                table: "DoctorSchedule",
                column: "DoctorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpecialty_Users_DoctorId",
                table: "DoctorSpecialty",
                column: "DoctorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_PatientProfile_PatientProfileId",
                table: "Invoice",
                column: "PatientProfileId",
                principalTable: "PatientProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecord_PatientProfile_PatientProfileId",
                table: "MedicalRecord",
                column: "PatientProfileId",
                principalTable: "PatientProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecord_Users_DoctorId",
                table: "MedicalRecord",
                column: "DoctorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_PatientProfile_PatientProfileId",
                table: "Payment",
                column: "PatientProfileId",
                principalTable: "PatientProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Users_CashierId",
                table: "Payment",
                column: "CashierId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Queue_PatientProfile_PatientProfileId",
                table: "Queue",
                column: "PatientProfileId",
                principalTable: "PatientProfile",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Queue_Users_ReceptionistId",
                table: "Queue",
                column: "ReceptionistId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Department_DepartmentId",
                table: "Users",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Department_Doctor_DepartmentId",
                table: "Users",
                column: "Doctor_DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Specialty_PrimarySpecialtyId",
                table: "Users",
                column: "PrimarySpecialtyId",
                principalTable: "Specialty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VitalSign_Users_NurseId",
                table: "VitalSign",
                column: "NurseId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
