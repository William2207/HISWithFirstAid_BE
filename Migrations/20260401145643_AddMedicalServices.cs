using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddMedicalServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Receptionists_CashierId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_CashierId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "CashierId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "ItemType",
                table: "InvoiceItem");

            migrationBuilder.AddColumn<int>(
                name: "SpecilityId",
                table: "InvoiceItem",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MedicalServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalServices", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItem_MedicalServiceId",
                table: "InvoiceItem",
                column: "MedicalServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItem_SpecilityId",
                table: "InvoiceItem",
                column: "SpecilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItem_MedicalServices_MedicalServiceId",
                table: "InvoiceItem",
                column: "MedicalServiceId",
                principalTable: "MedicalServices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceItem_Specialties_SpecilityId",
                table: "InvoiceItem",
                column: "SpecilityId",
                principalTable: "Specialties",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItem_MedicalServices_MedicalServiceId",
                table: "InvoiceItem");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceItem_Specialties_SpecilityId",
                table: "InvoiceItem");

            migrationBuilder.DropTable(
                name: "MedicalServices");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceItem_MedicalServiceId",
                table: "InvoiceItem");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceItem_SpecilityId",
                table: "InvoiceItem");

            migrationBuilder.DropColumn(
                name: "SpecilityId",
                table: "InvoiceItem");

            migrationBuilder.AddColumn<int>(
                name: "CashierId",
                table: "Payment",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemType",
                table: "InvoiceItem",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_CashierId",
                table: "Payment",
                column: "CashierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Receptionists_CashierId",
                table: "Payment",
                column: "CashierId",
                principalTable: "Receptionists",
                principalColumn: "Id");
        }
    }
}
