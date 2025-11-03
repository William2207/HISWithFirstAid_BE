using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class TechniqueAndQuizRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestions_TechniqueId",
                table: "QuizQuestions",
                column: "TechniqueId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizQuestions_Techniques_TechniqueId",
                table: "QuizQuestions",
                column: "TechniqueId",
                principalTable: "Techniques",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizQuestions_Techniques_TechniqueId",
                table: "QuizQuestions");

            migrationBuilder.DropIndex(
                name: "IX_QuizQuestions_TechniqueId",
                table: "QuizQuestions");
        }
    }
}
