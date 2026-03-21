using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class ModifyAppointmentMedicalRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Queues_QueueId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Queues_Receptionists_ReceptionistId",
                table: "Queues");

            migrationBuilder.DropIndex(
                name: "IX_Queues_ReceptionistId",
                table: "Queues");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_QueueId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "ReceptionistId",
                table: "Queues");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "FinalizedAt",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "AppointmentDate",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "AppointmentTime",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "CancellationReason",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "CheckedInAt",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "DocumentsHeld",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "DocumentsReturnedAt",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "QueueId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "StartedAt",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "DiagnosisICD10",
                table: "MedicalRecords",
                newName: "MedicalHistory");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Appointments",
                newName: "AppointmentDateTime");

            migrationBuilder.AddColumn<string>(
                name: "FamilyHistory",
                table: "MedicalRecords",
                type: "text",
                nullable: true);

            migrationBuilder.Sql(
                @"ALTER TABLE ""Appointments"" ALTER COLUMN ""Type"" TYPE integer
                  USING CASE ""Type""
                    WHEN 'WalkIn' THEN 0
                    WHEN 'Online' THEN 1
                    ELSE 0
                  END;"
             );

            migrationBuilder.Sql(
                @"ALTER TABLE ""Appointments"" ALTER COLUMN ""Status"" TYPE integer
                  USING CASE ""Status""
                    WHEN 'Registered' THEN 0
                    WHEN 'In_Progress' THEN 1
                    WHEN 'Completed' THEN 2
                    ELSE 0
                  END;"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FamilyHistory",
                table: "MedicalRecords");

            migrationBuilder.RenameColumn(
                name: "MedicalHistory",
                table: "MedicalRecords",
                newName: "DiagnosisICD10");

            migrationBuilder.RenameColumn(
                name: "AppointmentDateTime",
                table: "Appointments",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<int>(
                name: "ReceptionistId",
                table: "Queues",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Patients",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Patients",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FinalizedAt",
                table: "MedicalRecords",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "MedicalRecords",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Appointments",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Appointments",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "AppointmentDate",
                table: "Appointments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "AppointmentTime",
                table: "Appointments",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancellationReason",
                table: "Appointments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckedInAt",
                table: "Appointments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "Appointments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Appointments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "DocumentsHeld",
                table: "Appointments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DocumentsReturnedAt",
                table: "Appointments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Appointments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QueueId",
                table: "Appointments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedAt",
                table: "Appointments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Queues_ReceptionistId",
                table: "Queues",
                column: "ReceptionistId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_QueueId",
                table: "Appointments",
                column: "QueueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Queues_QueueId",
                table: "Appointments",
                column: "QueueId",
                principalTable: "Queues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Queues_Receptionists_ReceptionistId",
                table: "Queues",
                column: "ReceptionistId",
                principalTable: "Receptionists",
                principalColumn: "Id");
        }
    }
}
