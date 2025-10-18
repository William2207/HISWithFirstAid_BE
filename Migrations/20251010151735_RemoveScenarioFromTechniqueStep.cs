using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveScenarioFromTechniqueStep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechniqueSteps_Scenarios_ScenarioId",
                table: "TechniqueSteps");

            migrationBuilder.DropIndex(
                name: "IX_TechniqueSteps_ScenarioId",
                table: "TechniqueSteps");

            migrationBuilder.DropColumn(
                name: "ScenarioId",
                table: "TechniqueSteps");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScenarioId",
                table: "TechniqueSteps",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechniqueSteps_ScenarioId",
                table: "TechniqueSteps",
                column: "ScenarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_TechniqueSteps_Scenarios_ScenarioId",
                table: "TechniqueSteps",
                column: "ScenarioId",
                principalTable: "Scenarios",
                principalColumn: "Id");
        }
    }
}
