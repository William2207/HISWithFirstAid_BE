using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddHeadDoctorToSpeciality : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HeadDoctorId",
                table: "Specialties",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_HeadDoctorId",
                table: "Specialties",
                column: "HeadDoctorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Specialties_Doctors_HeadDoctorId",
                table: "Specialties",
                column: "HeadDoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Specialties_Doctors_HeadDoctorId",
                table: "Specialties");

            migrationBuilder.DropIndex(
                name: "IX_Specialties_HeadDoctorId",
                table: "Specialties");

            migrationBuilder.DropColumn(
                name: "HeadDoctorId",
                table: "Specialties");
        }
    }
}
