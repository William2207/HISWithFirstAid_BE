using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class ModifySpecilityInvoiceInvoiceItemPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Appointments_AppointmentId",
                table: "Invoices");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Specialties",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.Sql(@"
                ALTER TABLE ""Payment""
                ALTER COLUMN ""Status"" TYPE integer
                USING CASE ""Status""
                    WHEN 'Pending'    THEN 0
                    WHEN 'Processing' THEN 1
                    WHEN 'Completed'  THEN 2
                    WHEN 'Failed'     THEN 3
                    WHEN 'Refunded'   THEN 4
                    WHEN 'Cancelled'  THEN 5
                    ELSE 0
                END;
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""Payment""
                ALTER COLUMN ""PaymentMethod"" TYPE integer
                USING CASE ""PaymentMethod""
                    WHEN 'CreditCard'   THEN 0
                    WHEN 'DebitCard'    THEN 1
                    WHEN 'BankTransfer' THEN 2
                    WHEN 'Momo'         THEN 3
                    WHEN 'ZaloPay'      THEN 4
                    WHEN 'VNPay'        THEN 5
                    WHEN 'COD'          THEN 6
                    ELSE 0
                END;
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""Invoices""
                ALTER COLUMN ""Status"" TYPE integer
                USING CASE ""Status""
                    WHEN 'Pending'    THEN 0
                    WHEN 'Processing' THEN 1
                    WHEN 'Completed'  THEN 2
                    WHEN 'Cancelled'  THEN 3
                    ELSE 0
                END;
            ");

            migrationBuilder.AlterColumn<int>(
                name: "AppointmentId",
                table: "Invoices",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Appointments_AppointmentId",
                table: "Invoices",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Appointments_AppointmentId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Specialties");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Payment",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "Payment",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Invoices",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "AppointmentId",
                table: "Invoices",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Appointments_AppointmentId",
                table: "Invoices",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
