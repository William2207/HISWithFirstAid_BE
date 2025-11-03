using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateScenarioTechniqueRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Techniques_Scenarios_ScenarioId",
                table: "Techniques");

            migrationBuilder.DropIndex(
                name: "IX_Techniques_ScenarioId",
                table: "Techniques");

            migrationBuilder.DropColumn(
                name: "ScenarioId",
                table: "Techniques");

            migrationBuilder.CreateTable(
                name: "ScenarioTechnique",
                columns: table => new
                {
                    ScenarioId = table.Column<int>(type: "integer", nullable: false),
                    TechniqueId = table.Column<int>(type: "integer", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScenarioTechnique", x => new { x.ScenarioId, x.TechniqueId });
                    table.ForeignKey(
                        name: "FK_ScenarioTechnique_Scenarios_ScenarioId",
                        column: x => x.ScenarioId,
                        principalTable: "Scenarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScenarioTechnique_Techniques_TechniqueId",
                        column: x => x.TechniqueId,
                        principalTable: "Techniques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioTechnique_TechniqueId",
                table: "ScenarioTechnique",
                column: "TechniqueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScenarioTechnique");

            migrationBuilder.AddColumn<int>(
                name: "ScenarioId",
                table: "Techniques",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Techniques_ScenarioId",
                table: "Techniques",
                column: "ScenarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Techniques_Scenarios_ScenarioId",
                table: "Techniques",
                column: "ScenarioId",
                principalTable: "Scenarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
