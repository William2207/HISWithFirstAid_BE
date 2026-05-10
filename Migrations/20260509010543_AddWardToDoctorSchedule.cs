using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddWardToDoctorSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WardId",
                table: "DoctorSchedule",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSchedule_WardId",
                table: "DoctorSchedule",
                column: "WardId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSchedule_Wards_WardId",
                table: "DoctorSchedule",
                column: "WardId",
                principalTable: "Wards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSchedule_Wards_WardId",
                table: "DoctorSchedule");

            migrationBuilder.DropIndex(
                name: "IX_DoctorSchedule_WardId",
                table: "DoctorSchedule");

            migrationBuilder.DropColumn(
                name: "WardId",
                table: "DoctorSchedule");
        }
    }
}
