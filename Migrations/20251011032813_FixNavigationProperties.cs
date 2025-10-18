using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixNavigationProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Techniques_Scenarios_ScenarioId1",
                table: "Techniques");

            migrationBuilder.DropIndex(
                name: "IX_Techniques_ScenarioId1",
                table: "Techniques");

            migrationBuilder.DropColumn(
                name: "ScenarioId1",
                table: "Techniques");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScenarioId1",
                table: "Techniques",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Techniques_ScenarioId1",
                table: "Techniques",
                column: "ScenarioId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Techniques_Scenarios_ScenarioId1",
                table: "Techniques",
                column: "ScenarioId1",
                principalTable: "Scenarios",
                principalColumn: "Id");
        }
    }
}
