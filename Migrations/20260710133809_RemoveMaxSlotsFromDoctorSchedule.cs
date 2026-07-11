using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMaxSlotsFromDoctorSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxOnlineSlots",
                table: "DoctorSchedule");

            migrationBuilder.DropColumn(
                name: "MaxWalkInSlots",
                table: "DoctorSchedule");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxOnlineSlots",
                table: "DoctorSchedule",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxWalkInSlots",
                table: "DoctorSchedule",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
