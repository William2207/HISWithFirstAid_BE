using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTechniqueFromScenarioStep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScenarioSteps_Techniques_TechniqueId",
                table: "ScenarioSteps");

            migrationBuilder.DropIndex(
                name: "IX_ScenarioSteps_TechniqueId",
                table: "ScenarioSteps");

            migrationBuilder.DropColumn(
                name: "TechniqueId",
                table: "ScenarioSteps");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TechniqueId",
                table: "ScenarioSteps",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ScenarioSteps",
                keyColumn: "Id",
                keyValue: 1,
                column: "TechniqueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ScenarioSteps",
                keyColumn: "Id",
                keyValue: 2,
                column: "TechniqueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ScenarioSteps",
                keyColumn: "Id",
                keyValue: 3,
                column: "TechniqueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ScenarioSteps",
                keyColumn: "Id",
                keyValue: 4,
                column: "TechniqueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ScenarioSteps",
                keyColumn: "Id",
                keyValue: 5,
                column: "TechniqueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ScenarioSteps",
                keyColumn: "Id",
                keyValue: 6,
                column: "TechniqueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ScenarioSteps",
                keyColumn: "Id",
                keyValue: 7,
                column: "TechniqueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ScenarioSteps",
                keyColumn: "Id",
                keyValue: 8,
                column: "TechniqueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ScenarioSteps",
                keyColumn: "Id",
                keyValue: 9,
                column: "TechniqueId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ScenarioSteps",
                keyColumn: "Id",
                keyValue: 10,
                column: "TechniqueId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioSteps_TechniqueId",
                table: "ScenarioSteps",
                column: "TechniqueId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScenarioSteps_Techniques_TechniqueId",
                table: "ScenarioSteps",
                column: "TechniqueId",
                principalTable: "Techniques",
                principalColumn: "Id");
        }
    }
}
