using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddWardOrdersAndNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WardNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AdmissionRecordId = table.Column<int>(type: "integer", nullable: false),
                    AuthorUserId = table.Column<int>(type: "integer", nullable: false),
                    AuthorRole = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WardNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WardNotes_AdmissionRecords_AdmissionRecordId",
                        column: x => x.AdmissionRecordId,
                        principalTable: "AdmissionRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WardNotes_Users_AuthorUserId",
                        column: x => x.AuthorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WardOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AdmissionRecordId = table.Column<int>(type: "integer", nullable: false),
                    CreatedByDoctorId = table.Column<int>(type: "integer", nullable: false),
                    OrderType = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    ScheduledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WardOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WardOrders_AdmissionRecords_AdmissionRecordId",
                        column: x => x.AdmissionRecordId,
                        principalTable: "AdmissionRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WardOrders_Doctors_CreatedByDoctorId",
                        column: x => x.CreatedByDoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WardNotes_AdmissionRecordId_CreatedAt",
                table: "WardNotes",
                columns: new[] { "AdmissionRecordId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_WardNotes_AuthorUserId",
                table: "WardNotes",
                column: "AuthorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WardOrders_AdmissionRecordId_Status",
                table: "WardOrders",
                columns: new[] { "AdmissionRecordId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_WardOrders_CreatedByDoctorId",
                table: "WardOrders",
                column: "CreatedByDoctorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WardNotes");

            migrationBuilder.DropTable(
                name: "WardOrders");
        }
    }
}
