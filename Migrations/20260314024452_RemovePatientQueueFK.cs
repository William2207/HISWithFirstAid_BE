using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemovePatientQueueFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Queues_Patients_PatientId",
                table: "Queues");

            migrationBuilder.DropIndex(
                name: "IX_Queues_PatientId",
                table: "Queues");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Queues");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "Queues",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Queues_PatientId",
                table: "Queues",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Queues_Patients_PatientId",
                table: "Queues",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");
        }
    }
}
