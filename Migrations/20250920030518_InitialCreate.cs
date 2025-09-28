using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Techniques",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Difficulty = table.Column<string>(type: "text", nullable: false),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    ModelUrl = table.Column<string>(type: "text", nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Techniques", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScenarioSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TechniqueId = table.Column<int>(type: "integer", nullable: false),
                    StepNumber = table.Column<int>(type: "integer", nullable: false),
                    Instruction = table.Column<string>(type: "text", nullable: false),
                    ExpectedAction = table.Column<string>(type: "text", nullable: false),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScenarioSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScenarioSteps_Techniques_TechniqueId",
                        column: x => x.TechniqueId,
                        principalTable: "Techniques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioSteps_TechniqueId",
                table: "ScenarioSteps",
                column: "TechniqueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScenarioSteps");

            migrationBuilder.DropTable(
                name: "Techniques");
        }
    }
}
