using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateScenarioRelationshipContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScenarioStep_Scenarios_ScenarioId",
                table: "ScenarioStep");

            migrationBuilder.DropForeignKey(
                name: "FK_ScenarioStep_Techniques_TechniqueId",
                table: "ScenarioStep");

            migrationBuilder.DropForeignKey(
                name: "FK_ScenarioTechnique_Scenarios_ScenarioId",
                table: "ScenarioTechnique");

            migrationBuilder.DropForeignKey(
                name: "FK_ScenarioTechnique_Techniques_TechniqueId",
                table: "ScenarioTechnique");

            migrationBuilder.DropForeignKey(
                name: "FK_StepOption_ScenarioStep_StepId",
                table: "StepOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StepOption",
                table: "StepOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScenarioTechnique",
                table: "ScenarioTechnique");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScenarioStep",
                table: "ScenarioStep");

            migrationBuilder.RenameTable(
                name: "StepOption",
                newName: "StepOptions");

            migrationBuilder.RenameTable(
                name: "ScenarioTechnique",
                newName: "ScenarioTechniques");

            migrationBuilder.RenameTable(
                name: "ScenarioStep",
                newName: "ScenarioSteps");

            migrationBuilder.RenameIndex(
                name: "IX_StepOption_StepId",
                table: "StepOptions",
                newName: "IX_StepOptions_StepId");

            migrationBuilder.RenameIndex(
                name: "IX_ScenarioTechnique_TechniqueId",
                table: "ScenarioTechniques",
                newName: "IX_ScenarioTechniques_TechniqueId");

            migrationBuilder.RenameIndex(
                name: "IX_ScenarioStep_TechniqueId",
                table: "ScenarioSteps",
                newName: "IX_ScenarioSteps_TechniqueId");

            migrationBuilder.RenameIndex(
                name: "IX_ScenarioStep_ScenarioId",
                table: "ScenarioSteps",
                newName: "IX_ScenarioSteps_ScenarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StepOptions",
                table: "StepOptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScenarioTechniques",
                table: "ScenarioTechniques",
                columns: new[] { "ScenarioId", "TechniqueId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScenarioSteps",
                table: "ScenarioSteps",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScenarioSteps_Scenarios_ScenarioId",
                table: "ScenarioSteps",
                column: "ScenarioId",
                principalTable: "Scenarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScenarioSteps_Techniques_TechniqueId",
                table: "ScenarioSteps",
                column: "TechniqueId",
                principalTable: "Techniques",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScenarioTechniques_Scenarios_ScenarioId",
                table: "ScenarioTechniques",
                column: "ScenarioId",
                principalTable: "Scenarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScenarioTechniques_Techniques_TechniqueId",
                table: "ScenarioTechniques",
                column: "TechniqueId",
                principalTable: "Techniques",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StepOptions_ScenarioSteps_StepId",
                table: "StepOptions",
                column: "StepId",
                principalTable: "ScenarioSteps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScenarioSteps_Scenarios_ScenarioId",
                table: "ScenarioSteps");

            migrationBuilder.DropForeignKey(
                name: "FK_ScenarioSteps_Techniques_TechniqueId",
                table: "ScenarioSteps");

            migrationBuilder.DropForeignKey(
                name: "FK_ScenarioTechniques_Scenarios_ScenarioId",
                table: "ScenarioTechniques");

            migrationBuilder.DropForeignKey(
                name: "FK_ScenarioTechniques_Techniques_TechniqueId",
                table: "ScenarioTechniques");

            migrationBuilder.DropForeignKey(
                name: "FK_StepOptions_ScenarioSteps_StepId",
                table: "StepOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StepOptions",
                table: "StepOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScenarioTechniques",
                table: "ScenarioTechniques");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScenarioSteps",
                table: "ScenarioSteps");

            migrationBuilder.RenameTable(
                name: "StepOptions",
                newName: "StepOption");

            migrationBuilder.RenameTable(
                name: "ScenarioTechniques",
                newName: "ScenarioTechnique");

            migrationBuilder.RenameTable(
                name: "ScenarioSteps",
                newName: "ScenarioStep");

            migrationBuilder.RenameIndex(
                name: "IX_StepOptions_StepId",
                table: "StepOption",
                newName: "IX_StepOption_StepId");

            migrationBuilder.RenameIndex(
                name: "IX_ScenarioTechniques_TechniqueId",
                table: "ScenarioTechnique",
                newName: "IX_ScenarioTechnique_TechniqueId");

            migrationBuilder.RenameIndex(
                name: "IX_ScenarioSteps_TechniqueId",
                table: "ScenarioStep",
                newName: "IX_ScenarioStep_TechniqueId");

            migrationBuilder.RenameIndex(
                name: "IX_ScenarioSteps_ScenarioId",
                table: "ScenarioStep",
                newName: "IX_ScenarioStep_ScenarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StepOption",
                table: "StepOption",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScenarioTechnique",
                table: "ScenarioTechnique",
                columns: new[] { "ScenarioId", "TechniqueId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScenarioStep",
                table: "ScenarioStep",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScenarioStep_Scenarios_ScenarioId",
                table: "ScenarioStep",
                column: "ScenarioId",
                principalTable: "Scenarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScenarioStep_Techniques_TechniqueId",
                table: "ScenarioStep",
                column: "TechniqueId",
                principalTable: "Techniques",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScenarioTechnique_Scenarios_ScenarioId",
                table: "ScenarioTechnique",
                column: "ScenarioId",
                principalTable: "Scenarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScenarioTechnique_Techniques_TechniqueId",
                table: "ScenarioTechnique",
                column: "TechniqueId",
                principalTable: "Techniques",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StepOption_ScenarioStep_StepId",
                table: "StepOption",
                column: "StepId",
                principalTable: "ScenarioStep",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
