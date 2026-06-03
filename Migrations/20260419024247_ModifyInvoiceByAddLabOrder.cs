using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class ModifyInvoiceByAddLabOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LabOrderId",
                table: "Invoices",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LabOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AppointmentId = table.Column<int>(type: "integer", nullable: false),
                    DoctorId = table.Column<int>(type: "integer", nullable: false),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabOrder_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LabOrderItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LabOrderId = table.Column<int>(type: "integer", nullable: false),
                    MedicalServiceId = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabOrderItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabOrderItem_LabOrder_LabOrderId",
                        column: x => x.LabOrderId,
                        principalTable: "LabOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabOrderItem_MedicalServices_MedicalServiceId",
                        column: x => x.MedicalServiceId,
                        principalTable: "MedicalServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_LabOrderId",
                table: "Invoices",
                column: "LabOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_LabOrder_AppointmentId",
                table: "LabOrder",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_LabOrderItem_LabOrderId",
                table: "LabOrderItem",
                column: "LabOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_LabOrderItem_MedicalServiceId",
                table: "LabOrderItem",
                column: "MedicalServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_LabOrder_LabOrderId",
                table: "Invoices",
                column: "LabOrderId",
                principalTable: "LabOrder",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_LabOrder_LabOrderId",
                table: "Invoices");

            migrationBuilder.DropTable(
                name: "LabOrderItem");

            migrationBuilder.DropTable(
                name: "LabOrder");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_LabOrderId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "LabOrderId",
                table: "Invoices");
        }
    }
}
