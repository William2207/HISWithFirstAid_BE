using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class SyncTableNamesWithDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Doctor_DoctorId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Patient_PatientId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Queue_QueueId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Room_RoomId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Specialty_SpecialtyId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Users_CreatorId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Bed_Patient_CurrentPatientId",
                table: "Bed");

            migrationBuilder.DropForeignKey(
                name: "FK_Bed_Room_RoomId",
                table: "Bed");

            migrationBuilder.DropForeignKey(
                name: "FK_Department_Doctor_HeadDoctorId",
                table: "Department");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctor_Department_DepartmentId",
                table: "Doctor");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctor_Specialty_PrimarySpecialtyId",
                table: "Doctor");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctor_Users_UserId",
                table: "Doctor");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSchedule_Doctor_DoctorId",
                table: "DoctorSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSchedule_Room_RoomId",
                table: "DoctorSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpecialty_Doctor_DoctorId",
                table: "DoctorSpecialty");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpecialty_Specialty_SpecialtyId",
                table: "DoctorSpecialty");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Appointment_AppointmentId",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Patient_PatientId",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItem_Invoice_InvoiceId",
                table: "InvoiceItem");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecord_Appointment_AppointmentId",
                table: "MedicalRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecord_Doctor_DoctorId",
                table: "MedicalRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecord_Patient_PatientId",
                table: "MedicalRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_Nurse_Department_DepartmentId",
                table: "Nurse");

            migrationBuilder.DropForeignKey(
                name: "FK_Nurse_Users_UserId",
                table: "Nurse");

            migrationBuilder.DropForeignKey(
                name: "FK_Patient_Receptionist_ReceptionistId",
                table: "Patient");

            migrationBuilder.DropForeignKey(
                name: "FK_Patient_Users_CreatorId",
                table: "Patient");

            migrationBuilder.DropForeignKey(
                name: "FK_Patient_Users_UserId",
                table: "Patient");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Invoice_InvoiceId",
                table: "Payment");

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
                name: "FK_Receptionist_Users_UserId",
                table: "Receptionist");

            migrationBuilder.DropForeignKey(
                name: "FK_Room_Department_DepartmentId",
                table: "Room");

            migrationBuilder.DropForeignKey(
                name: "FK_Specialty_Department_DepartmentId",
                table: "Specialty");

            migrationBuilder.DropForeignKey(
                name: "FK_VitalSign_MedicalRecord_MedicalRecordId",
                table: "VitalSign");

            migrationBuilder.DropForeignKey(
                name: "FK_VitalSign_Nurse_NurseId",
                table: "VitalSign");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VitalSign",
                table: "VitalSign");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Specialty",
                table: "Specialty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Room",
                table: "Room");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Receptionist",
                table: "Receptionist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Queue",
                table: "Queue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Patient",
                table: "Patient");

            migrationBuilder.DropIndex(
                name: "IX_Patient_CreatorId",
                table: "Patient");

            migrationBuilder.DropIndex(
                name: "IX_Patient_ReceptionistId",
                table: "Patient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Nurse",
                table: "Nurse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicalRecord",
                table: "MedicalRecord");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invoice",
                table: "Invoice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorSpecialty",
                table: "DoctorSpecialty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Doctor",
                table: "Doctor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Department",
                table: "Department");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bed",
                table: "Bed");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointment",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_QueueId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "ReceptionistId",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "ConsultationFee",
                table: "Doctor");

            migrationBuilder.RenameTable(
                name: "VitalSign",
                newName: "VitalSigns");

            migrationBuilder.RenameTable(
                name: "Specialty",
                newName: "Specialties");

            migrationBuilder.RenameTable(
                name: "Room",
                newName: "Rooms");

            migrationBuilder.RenameTable(
                name: "Receptionist",
                newName: "Receptionists");

            migrationBuilder.RenameTable(
                name: "Queue",
                newName: "Queues");

            migrationBuilder.RenameTable(
                name: "Patient",
                newName: "Patients");

            migrationBuilder.RenameTable(
                name: "Nurse",
                newName: "Nurses");

            migrationBuilder.RenameTable(
                name: "MedicalRecord",
                newName: "MedicalRecords");

            migrationBuilder.RenameTable(
                name: "Invoice",
                newName: "Invoices");

            migrationBuilder.RenameTable(
                name: "DoctorSpecialty",
                newName: "DoctorSpecialties");

            migrationBuilder.RenameTable(
                name: "Doctor",
                newName: "Doctors");

            migrationBuilder.RenameTable(
                name: "Department",
                newName: "Departments");

            migrationBuilder.RenameTable(
                name: "Bed",
                newName: "Beds");

            migrationBuilder.RenameTable(
                name: "Appointment",
                newName: "Appointments");

            migrationBuilder.RenameIndex(
                name: "IX_VitalSign_NurseId",
                table: "VitalSigns",
                newName: "IX_VitalSigns_NurseId");

            migrationBuilder.RenameIndex(
                name: "IX_VitalSign_MedicalRecordId",
                table: "VitalSigns",
                newName: "IX_VitalSigns_MedicalRecordId");

            migrationBuilder.RenameIndex(
                name: "IX_Specialty_DepartmentId",
                table: "Specialties",
                newName: "IX_Specialties_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Room_DepartmentId",
                table: "Rooms",
                newName: "IX_Rooms_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Receptionist_UserId",
                table: "Receptionists",
                newName: "IX_Receptionists_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Queue_ReceptionistId",
                table: "Queues",
                newName: "IX_Queues_ReceptionistId");

            migrationBuilder.RenameIndex(
                name: "IX_Queue_PatientId",
                table: "Queues",
                newName: "IX_Queues_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Patient_UserId",
                table: "Patients",
                newName: "IX_Patients_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Nurse_UserId",
                table: "Nurses",
                newName: "IX_Nurses_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Nurse_DepartmentId",
                table: "Nurses",
                newName: "IX_Nurses_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalRecord_PatientId",
                table: "MedicalRecords",
                newName: "IX_MedicalRecords_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalRecord_DoctorId",
                table: "MedicalRecords",
                newName: "IX_MedicalRecords_DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalRecord_AppointmentId",
                table: "MedicalRecords",
                newName: "IX_MedicalRecords_AppointmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoice_PatientId",
                table: "Invoices",
                newName: "IX_Invoices_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoice_AppointmentId",
                table: "Invoices",
                newName: "IX_Invoices_AppointmentId");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorSpecialty_SpecialtyId",
                table: "DoctorSpecialties",
                newName: "IX_DoctorSpecialties_SpecialtyId");

            migrationBuilder.RenameIndex(
                name: "IX_Doctor_UserId",
                table: "Doctors",
                newName: "IX_Doctors_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Doctor_PrimarySpecialtyId",
                table: "Doctors",
                newName: "IX_Doctors_PrimarySpecialtyId");

            migrationBuilder.RenameIndex(
                name: "IX_Doctor_DepartmentId",
                table: "Doctors",
                newName: "IX_Doctors_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Department_HeadDoctorId",
                table: "Departments",
                newName: "IX_Departments_HeadDoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_Bed_RoomId",
                table: "Beds",
                newName: "IX_Beds_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Bed_CurrentPatientId",
                table: "Beds",
                newName: "IX_Beds_CurrentPatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_SpecialtyId",
                table: "Appointments",
                newName: "IX_Appointments_SpecialtyId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_RoomId",
                table: "Appointments",
                newName: "IX_Appointments_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_PatientId",
                table: "Appointments",
                newName: "IX_Appointments_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_DoctorId",
                table: "Appointments",
                newName: "IX_Appointments_DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_CreatorId",
                table: "Appointments",
                newName: "IX_Appointments_CreatorId");

            migrationBuilder.Sql(
                @"ALTER TABLE ""Queues""
                  ALTER COLUMN ""QueueNumber""
                  TYPE integer
                  USING (""QueueNumber""::integer);"
            );

            migrationBuilder.AddColumn<DateOnly>(
                name: "QueueDate",
                table: "Queues",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddPrimaryKey(
                name: "PK_VitalSigns",
                table: "VitalSigns",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Specialties",
                table: "Specialties",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Receptionists",
                table: "Receptionists",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Queues",
                table: "Queues",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Patients",
                table: "Patients",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Nurses",
                table: "Nurses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicalRecords",
                table: "MedicalRecords",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invoices",
                table: "Invoices",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorSpecialties",
                table: "DoctorSpecialties",
                columns: new[] { "DoctorId", "SpecialtyId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Doctors",
                table: "Doctors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Departments",
                table: "Departments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Beds",
                table: "Beds",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Queues_QueueDate",
                table: "Queues",
                column: "QueueDate");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_QueueId",
                table: "Appointments",
                column: "QueueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Doctors_DoctorId",
                table: "Appointments",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Queues_QueueId",
                table: "Appointments",
                column: "QueueId",
                principalTable: "Queues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Rooms_RoomId",
                table: "Appointments",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Specialties_SpecialtyId",
                table: "Appointments",
                column: "SpecialtyId",
                principalTable: "Specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Users_CreatorId",
                table: "Appointments",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Beds_Patients_CurrentPatientId",
                table: "Beds",
                column: "CurrentPatientId",
                principalTable: "Patients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Beds_Rooms_RoomId",
                table: "Beds",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Doctors_HeadDoctorId",
                table: "Departments",
                column: "HeadDoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Departments_DepartmentId",
                table: "Doctors",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Specialties_PrimarySpecialtyId",
                table: "Doctors",
                column: "PrimarySpecialtyId",
                principalTable: "Specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Users_UserId",
                table: "Doctors",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSchedule_Doctors_DoctorId",
                table: "DoctorSchedule",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSchedule_Rooms_RoomId",
                table: "DoctorSchedule",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpecialties_Doctors_DoctorId",
                table: "DoctorSpecialties",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpecialties_Specialties_SpecialtyId",
                table: "DoctorSpecialties",
                column: "SpecialtyId",
                principalTable: "Specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItem_Invoices_InvoiceId",
                table: "InvoiceItem",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Appointments_AppointmentId",
                table: "Invoices",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Patients_PatientId",
                table: "Invoices",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_Appointments_AppointmentId",
                table: "MedicalRecords",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_Doctors_DoctorId",
                table: "MedicalRecords",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_Patients_PatientId",
                table: "MedicalRecords",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Nurses_Departments_DepartmentId",
                table: "Nurses",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Nurses_Users_UserId",
                table: "Nurses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Users_UserId",
                table: "Patients",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Invoices_InvoiceId",
                table: "Payment",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Patients_PatientId",
                table: "Payment",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Receptionists_CashierId",
                table: "Payment",
                column: "CashierId",
                principalTable: "Receptionists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Queues_Patients_PatientId",
                table: "Queues",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Queues_Receptionists_ReceptionistId",
                table: "Queues",
                column: "ReceptionistId",
                principalTable: "Receptionists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Receptionists_Users_UserId",
                table: "Receptionists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Departments_DepartmentId",
                table: "Rooms",
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
                name: "FK_VitalSigns_MedicalRecords_MedicalRecordId",
                table: "VitalSigns",
                column: "MedicalRecordId",
                principalTable: "MedicalRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VitalSigns_Nurses_NurseId",
                table: "VitalSigns",
                column: "NurseId",
                principalTable: "Nurses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Doctors_DoctorId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Queues_QueueId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Rooms_RoomId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Specialties_SpecialtyId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Users_CreatorId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Beds_Patients_CurrentPatientId",
                table: "Beds");

            migrationBuilder.DropForeignKey(
                name: "FK_Beds_Rooms_RoomId",
                table: "Beds");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Doctors_HeadDoctorId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Departments_DepartmentId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Specialties_PrimarySpecialtyId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Users_UserId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSchedule_Doctors_DoctorId",
                table: "DoctorSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSchedule_Rooms_RoomId",
                table: "DoctorSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpecialties_Doctors_DoctorId",
                table: "DoctorSpecialties");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpecialties_Specialties_SpecialtyId",
                table: "DoctorSpecialties");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItem_Invoices_InvoiceId",
                table: "InvoiceItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Appointments_AppointmentId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Patients_PatientId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_Appointments_AppointmentId",
                table: "MedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_Doctors_DoctorId",
                table: "MedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_Patients_PatientId",
                table: "MedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Nurses_Departments_DepartmentId",
                table: "Nurses");

            migrationBuilder.DropForeignKey(
                name: "FK_Nurses_Users_UserId",
                table: "Nurses");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Users_UserId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Invoices_InvoiceId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Patients_PatientId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Receptionists_CashierId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Queues_Patients_PatientId",
                table: "Queues");

            migrationBuilder.DropForeignKey(
                name: "FK_Queues_Receptionists_ReceptionistId",
                table: "Queues");

            migrationBuilder.DropForeignKey(
                name: "FK_Receptionists_Users_UserId",
                table: "Receptionists");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Departments_DepartmentId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Specialties_Departments_DepartmentId",
                table: "Specialties");

            migrationBuilder.DropForeignKey(
                name: "FK_VitalSigns_MedicalRecords_MedicalRecordId",
                table: "VitalSigns");

            migrationBuilder.DropForeignKey(
                name: "FK_VitalSigns_Nurses_NurseId",
                table: "VitalSigns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VitalSigns",
                table: "VitalSigns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Specialties",
                table: "Specialties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Receptionists",
                table: "Receptionists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Queues",
                table: "Queues");

            migrationBuilder.DropIndex(
                name: "IX_Queues_QueueDate",
                table: "Queues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Patients",
                table: "Patients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Nurses",
                table: "Nurses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicalRecords",
                table: "MedicalRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invoices",
                table: "Invoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorSpecialties",
                table: "DoctorSpecialties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Doctors",
                table: "Doctors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Departments",
                table: "Departments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Beds",
                table: "Beds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_QueueId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "QueueDate",
                table: "Queues");

            migrationBuilder.RenameTable(
                name: "VitalSigns",
                newName: "VitalSign");

            migrationBuilder.RenameTable(
                name: "Specialties",
                newName: "Specialty");

            migrationBuilder.RenameTable(
                name: "Rooms",
                newName: "Room");

            migrationBuilder.RenameTable(
                name: "Receptionists",
                newName: "Receptionist");

            migrationBuilder.RenameTable(
                name: "Queues",
                newName: "Queue");

            migrationBuilder.RenameTable(
                name: "Patients",
                newName: "Patient");

            migrationBuilder.RenameTable(
                name: "Nurses",
                newName: "Nurse");

            migrationBuilder.RenameTable(
                name: "MedicalRecords",
                newName: "MedicalRecord");

            migrationBuilder.RenameTable(
                name: "Invoices",
                newName: "Invoice");

            migrationBuilder.RenameTable(
                name: "DoctorSpecialties",
                newName: "DoctorSpecialty");

            migrationBuilder.RenameTable(
                name: "Doctors",
                newName: "Doctor");

            migrationBuilder.RenameTable(
                name: "Departments",
                newName: "Department");

            migrationBuilder.RenameTable(
                name: "Beds",
                newName: "Bed");

            migrationBuilder.RenameTable(
                name: "Appointments",
                newName: "Appointment");

            migrationBuilder.RenameIndex(
                name: "IX_VitalSigns_NurseId",
                table: "VitalSign",
                newName: "IX_VitalSign_NurseId");

            migrationBuilder.RenameIndex(
                name: "IX_VitalSigns_MedicalRecordId",
                table: "VitalSign",
                newName: "IX_VitalSign_MedicalRecordId");

            migrationBuilder.RenameIndex(
                name: "IX_Specialties_DepartmentId",
                table: "Specialty",
                newName: "IX_Specialty_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_DepartmentId",
                table: "Room",
                newName: "IX_Room_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Receptionists_UserId",
                table: "Receptionist",
                newName: "IX_Receptionist_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Queues_ReceptionistId",
                table: "Queue",
                newName: "IX_Queue_ReceptionistId");

            migrationBuilder.RenameIndex(
                name: "IX_Queues_PatientId",
                table: "Queue",
                newName: "IX_Queue_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Patients_UserId",
                table: "Patient",
                newName: "IX_Patient_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Nurses_UserId",
                table: "Nurse",
                newName: "IX_Nurse_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Nurses_DepartmentId",
                table: "Nurse",
                newName: "IX_Nurse_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalRecords_PatientId",
                table: "MedicalRecord",
                newName: "IX_MedicalRecord_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalRecords_DoctorId",
                table: "MedicalRecord",
                newName: "IX_MedicalRecord_DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalRecords_AppointmentId",
                table: "MedicalRecord",
                newName: "IX_MedicalRecord_AppointmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_PatientId",
                table: "Invoice",
                newName: "IX_Invoice_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_AppointmentId",
                table: "Invoice",
                newName: "IX_Invoice_AppointmentId");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorSpecialties_SpecialtyId",
                table: "DoctorSpecialty",
                newName: "IX_DoctorSpecialty_SpecialtyId");

            migrationBuilder.RenameIndex(
                name: "IX_Doctors_UserId",
                table: "Doctor",
                newName: "IX_Doctor_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Doctors_PrimarySpecialtyId",
                table: "Doctor",
                newName: "IX_Doctor_PrimarySpecialtyId");

            migrationBuilder.RenameIndex(
                name: "IX_Doctors_DepartmentId",
                table: "Doctor",
                newName: "IX_Doctor_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Departments_HeadDoctorId",
                table: "Department",
                newName: "IX_Department_HeadDoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_Beds_RoomId",
                table: "Bed",
                newName: "IX_Bed_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Beds_CurrentPatientId",
                table: "Bed",
                newName: "IX_Bed_CurrentPatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_SpecialtyId",
                table: "Appointment",
                newName: "IX_Appointment_SpecialtyId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_RoomId",
                table: "Appointment",
                newName: "IX_Appointment_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointment",
                newName: "IX_Appointment_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointment",
                newName: "IX_Appointment_DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_CreatorId",
                table: "Appointment",
                newName: "IX_Appointment_CreatorId");

            migrationBuilder.AlterColumn<string>(
                name: "QueueNumber",
                table: "Queue",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "Patient",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Patient",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Patient",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReceptionistId",
                table: "Patient",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ConsultationFee",
                table: "Doctor",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VitalSign",
                table: "VitalSign",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Specialty",
                table: "Specialty",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Room",
                table: "Room",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Receptionist",
                table: "Receptionist",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Queue",
                table: "Queue",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Patient",
                table: "Patient",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Nurse",
                table: "Nurse",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicalRecord",
                table: "MedicalRecord",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invoice",
                table: "Invoice",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorSpecialty",
                table: "DoctorSpecialty",
                columns: new[] { "DoctorId", "SpecialtyId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Doctor",
                table: "Doctor",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Department",
                table: "Department",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bed",
                table: "Bed",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointment",
                table: "Appointment",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_CreatorId",
                table: "Patient",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_ReceptionistId",
                table: "Patient",
                column: "ReceptionistId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_QueueId",
                table: "Appointment",
                column: "QueueId",
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
                name: "FK_Appointment_Queue_QueueId",
                table: "Appointment",
                column: "QueueId",
                principalTable: "Queue",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Room_RoomId",
                table: "Appointment",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Specialty_SpecialtyId",
                table: "Appointment",
                column: "SpecialtyId",
                principalTable: "Specialty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Users_CreatorId",
                table: "Appointment",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bed_Patient_CurrentPatientId",
                table: "Bed",
                column: "CurrentPatientId",
                principalTable: "Patient",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bed_Room_RoomId",
                table: "Bed",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Department_Doctor_HeadDoctorId",
                table: "Department",
                column: "HeadDoctorId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctor_Department_DepartmentId",
                table: "Doctor",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctor_Specialty_PrimarySpecialtyId",
                table: "Doctor",
                column: "PrimarySpecialtyId",
                principalTable: "Specialty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctor_Users_UserId",
                table: "Doctor",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSchedule_Doctor_DoctorId",
                table: "DoctorSchedule",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSchedule_Room_RoomId",
                table: "DoctorSchedule",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpecialty_Doctor_DoctorId",
                table: "DoctorSpecialty",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpecialty_Specialty_SpecialtyId",
                table: "DoctorSpecialty",
                column: "SpecialtyId",
                principalTable: "Specialty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Appointment_AppointmentId",
                table: "Invoice",
                column: "AppointmentId",
                principalTable: "Appointment",
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
                name: "FK_InvoiceItem_Invoice_InvoiceId",
                table: "InvoiceItem",
                column: "InvoiceId",
                principalTable: "Invoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecord_Appointment_AppointmentId",
                table: "MedicalRecord",
                column: "AppointmentId",
                principalTable: "Appointment",
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
                name: "FK_Nurse_Department_DepartmentId",
                table: "Nurse",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Nurse_Users_UserId",
                table: "Nurse",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Patient_Receptionist_ReceptionistId",
                table: "Patient",
                column: "ReceptionistId",
                principalTable: "Receptionist",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Patient_Users_CreatorId",
                table: "Patient",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Patient_Users_UserId",
                table: "Patient",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Invoice_InvoiceId",
                table: "Payment",
                column: "InvoiceId",
                principalTable: "Invoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_Receptionist_Users_UserId",
                table: "Receptionist",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Room_Department_DepartmentId",
                table: "Room",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Specialty_Department_DepartmentId",
                table: "Specialty",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VitalSign_MedicalRecord_MedicalRecordId",
                table: "VitalSign",
                column: "MedicalRecordId",
                principalTable: "MedicalRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VitalSign_Nurse_NurseId",
                table: "VitalSign",
                column: "NurseId",
                principalTable: "Nurse",
                principalColumn: "Id");
        }
    }
}
