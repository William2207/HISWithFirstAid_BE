using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddHeadNurseToSpeciality : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HeadNurseId",
                table: "Specialties",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_HeadNurseId",
                table: "Specialties",
                column: "HeadNurseId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Specialties_Nurses_HeadNurseId",
                table: "Specialties",
                column: "HeadNurseId",
                principalTable: "Nurses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Specialties_Nurses_HeadNurseId",
                table: "Specialties");

            migrationBuilder.DropIndex(
                name: "IX_Specialties_HeadNurseId",
                table: "Specialties");

            migrationBuilder.DropColumn(
                name: "HeadNurseId",
                table: "Specialties");
        }
    }
}
