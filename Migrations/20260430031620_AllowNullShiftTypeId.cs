using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class AllowNullShiftTypeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSchedule_ShiftTypes_ShiftTypeId",
                table: "DoctorSchedule");

            migrationBuilder.AlterColumn<int>(
                name: "ShiftTypeId",
                table: "DoctorSchedule",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSchedule_ShiftTypes_ShiftTypeId",
                table: "DoctorSchedule",
                column: "ShiftTypeId",
                principalTable: "ShiftTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSchedule_ShiftTypes_ShiftTypeId",
                table: "DoctorSchedule");

            migrationBuilder.AlterColumn<int>(
                name: "ShiftTypeId",
                table: "DoctorSchedule",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSchedule_ShiftTypes_ShiftTypeId",
                table: "DoctorSchedule",
                column: "ShiftTypeId",
                principalTable: "ShiftTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
