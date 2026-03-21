using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRoomAddClinicWard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Rooms_RoomId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Beds_Rooms_RoomId",
                table: "Beds");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSchedule_Rooms_RoomId",
                table: "DoctorSchedule");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "DoctorSchedule",
                newName: "ClinicId");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorSchedule_RoomId",
                table: "DoctorSchedule",
                newName: "IX_DoctorSchedule_ClinicId");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Beds",
                newName: "WardId");

            migrationBuilder.RenameIndex(
                name: "IX_Beds_RoomId",
                table: "Beds",
                newName: "IX_Beds_WardId");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Appointments",
                newName: "ClinicId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_RoomId",
                table: "Appointments",
                newName: "IX_Appointments_ClinicId");

            migrationBuilder.CreateTable(
                name: "Clinics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoomNumber = table.Column<string>(type: "text", nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    SpecialtyId = table.Column<int>(type: "integer", nullable: false),
                    Floor = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clinics_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clinics_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoomNumber = table.Column<string>(type: "text", nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    WardType = table.Column<string>(type: "text", nullable: false),
                    Floor = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wards_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clinics_DepartmentId",
                table: "Clinics",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Clinics_SpecialtyId",
                table: "Clinics",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_Wards_DepartmentId",
                table: "Wards",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Clinics_ClinicId",
                table: "Appointments",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Beds_Wards_WardId",
                table: "Beds",
                column: "WardId",
                principalTable: "Wards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSchedule_Clinics_ClinicId",
                table: "DoctorSchedule",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Clinics_ClinicId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Beds_Wards_WardId",
                table: "Beds");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSchedule_Clinics_ClinicId",
                table: "DoctorSchedule");

            migrationBuilder.DropTable(
                name: "Clinics");

            migrationBuilder.DropTable(
                name: "Wards");

            migrationBuilder.RenameColumn(
                name: "ClinicId",
                table: "DoctorSchedule",
                newName: "RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorSchedule_ClinicId",
                table: "DoctorSchedule",
                newName: "IX_DoctorSchedule_RoomId");

            migrationBuilder.RenameColumn(
                name: "WardId",
                table: "Beds",
                newName: "RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Beds_WardId",
                table: "Beds",
                newName: "IX_Beds_RoomId");

            migrationBuilder.RenameColumn(
                name: "ClinicId",
                table: "Appointments",
                newName: "RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_ClinicId",
                table: "Appointments",
                newName: "IX_Appointments_RoomId");

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    RoomNumber = table.Column<string>(type: "text", nullable: false),
                    RoomType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_DepartmentId",
                table: "Rooms",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Rooms_RoomId",
                table: "Appointments",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Beds_Rooms_RoomId",
                table: "Beds",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSchedule_Rooms_RoomId",
                table: "DoctorSchedule",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }
    }
}
