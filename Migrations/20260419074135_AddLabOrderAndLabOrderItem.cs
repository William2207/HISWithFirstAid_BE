using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddLabOrderAndLabOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_LabOrder_LabOrderId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_LabOrder_Appointments_AppointmentId",
                table: "LabOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_LabOrderItem_LabOrder_LabOrderId",
                table: "LabOrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_LabOrderItem_MedicalServices_MedicalServiceId",
                table: "LabOrderItem");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_LabOrderId",
                table: "Invoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LabOrderItem",
                table: "LabOrderItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LabOrder",
                table: "LabOrder");

            migrationBuilder.RenameTable(
                name: "LabOrderItem",
                newName: "LabOrderItems");

            migrationBuilder.RenameTable(
                name: "LabOrder",
                newName: "LabOrders");

            migrationBuilder.RenameIndex(
                name: "IX_LabOrderItem_MedicalServiceId",
                table: "LabOrderItems",
                newName: "IX_LabOrderItems_MedicalServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_LabOrderItem_LabOrderId",
                table: "LabOrderItems",
                newName: "IX_LabOrderItems_LabOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_LabOrder_AppointmentId",
                table: "LabOrders",
                newName: "IX_LabOrders_AppointmentId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "MedicalServices",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "LabOrderItems",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "LabOrderItems",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "LabOrderItems",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "LabOrders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LabOrderItems",
                table: "LabOrderItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LabOrders",
                table: "LabOrders",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_LabOrderId",
                table: "Invoices",
                column: "LabOrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_LabOrders_LabOrderId",
                table: "Invoices",
                column: "LabOrderId",
                principalTable: "LabOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_LabOrderItems_LabOrders_LabOrderId",
                table: "LabOrderItems",
                column: "LabOrderId",
                principalTable: "LabOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LabOrderItems_MedicalServices_MedicalServiceId",
                table: "LabOrderItems",
                column: "MedicalServiceId",
                principalTable: "MedicalServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LabOrders_Appointments_AppointmentId",
                table: "LabOrders",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_LabOrders_LabOrderId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_LabOrderItems_LabOrders_LabOrderId",
                table: "LabOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_LabOrderItems_MedicalServices_MedicalServiceId",
                table: "LabOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_LabOrders_Appointments_AppointmentId",
                table: "LabOrders");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_LabOrderId",
                table: "Invoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LabOrders",
                table: "LabOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LabOrderItems",
                table: "LabOrderItems");

            migrationBuilder.RenameTable(
                name: "LabOrders",
                newName: "LabOrder");

            migrationBuilder.RenameTable(
                name: "LabOrderItems",
                newName: "LabOrderItem");

            migrationBuilder.RenameIndex(
                name: "IX_LabOrders_AppointmentId",
                table: "LabOrder",
                newName: "IX_LabOrder_AppointmentId");

            migrationBuilder.RenameIndex(
                name: "IX_LabOrderItems_MedicalServiceId",
                table: "LabOrderItem",
                newName: "IX_LabOrderItem_MedicalServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_LabOrderItems_LabOrderId",
                table: "LabOrderItem",
                newName: "IX_LabOrderItem_LabOrderId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "MedicalServices",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "LabOrder",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW()");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "LabOrderItem",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "LabOrderItem",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "LabOrderItem",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LabOrder",
                table: "LabOrder",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LabOrderItem",
                table: "LabOrderItem",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_LabOrderId",
                table: "Invoices",
                column: "LabOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_LabOrder_LabOrderId",
                table: "Invoices",
                column: "LabOrderId",
                principalTable: "LabOrder",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LabOrder_Appointments_AppointmentId",
                table: "LabOrder",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LabOrderItem_LabOrder_LabOrderId",
                table: "LabOrderItem",
                column: "LabOrderId",
                principalTable: "LabOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LabOrderItem_MedicalServices_MedicalServiceId",
                table: "LabOrderItem",
                column: "MedicalServiceId",
                principalTable: "MedicalServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
