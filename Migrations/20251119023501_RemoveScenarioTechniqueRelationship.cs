using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveScenarioTechniqueRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScenarioTechniques");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScenarioTechniques",
                columns: table => new
                {
                    ScenarioId = table.Column<int>(type: "integer", nullable: false),
                    TechniqueId = table.Column<int>(type: "integer", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScenarioTechniques", x => new { x.ScenarioId, x.TechniqueId });
                    table.ForeignKey(
                        name: "FK_ScenarioTechniques_Scenarios_ScenarioId",
                        column: x => x.ScenarioId,
                        principalTable: "Scenarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScenarioTechniques_Techniques_TechniqueId",
                        column: x => x.TechniqueId,
                        principalTable: "Techniques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ScenarioTechniques",
                columns: new[] { "ScenarioId", "TechniqueId", "Order" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 1, 4, 2 },
                    { 1, 16, 3 },
                    { 2, 2, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioTechniques_TechniqueId",
                table: "ScenarioTechniques",
                column: "TechniqueId");
        }
    }
}
