using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddScenariosAndQuiz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Techniques",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ThumbnailUrl",
                table: "Techniques",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Techniques",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ModelUrl",
                table: "Techniques",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Difficulty",
                table: "Techniques",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Techniques",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "TechniqueId",
                table: "ScenarioSteps",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Instruction",
                table: "ScenarioSteps",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "ScenarioSteps",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ExpectedAction",
                table: "ScenarioSteps",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "ScenarioId",
                table: "ScenarioSteps",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScenarioId1",
                table: "ScenarioSteps",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TechniqueId1",
                table: "ScenarioSteps",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "QuizQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TechniqueId = table.Column<int>(type: "integer", nullable: false),
                    QuestionText = table.Column<string>(type: "text", nullable: true),
                    Difficulty = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizQuestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Scenarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    Difficulty = table.Column<string>(type: "text", nullable: true),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    ModelUrl = table.Column<string>(type: "text", nullable: true),
                    ThumbnailUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scenarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnswerOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuizQuestionId = table.Column<int>(type: "integer", nullable: false),
                    AnswerText = table.Column<string>(type: "text", nullable: true),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerOptions_QuizQuestions_QuizQuestionId",
                        column: x => x.QuizQuestionId,
                        principalTable: "QuizQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioSteps_ScenarioId",
                table: "ScenarioSteps",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioSteps_ScenarioId1",
                table: "ScenarioSteps",
                column: "ScenarioId1");

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioSteps_TechniqueId1",
                table: "ScenarioSteps",
                column: "TechniqueId1");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOptions_QuizQuestionId",
                table: "AnswerOptions",
                column: "QuizQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScenarioSteps_Scenarios_ScenarioId",
                table: "ScenarioSteps",
                column: "ScenarioId",
                principalTable: "Scenarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScenarioSteps_Scenarios_ScenarioId1",
                table: "ScenarioSteps",
                column: "ScenarioId1",
                principalTable: "Scenarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScenarioSteps_Techniques_TechniqueId1",
                table: "ScenarioSteps",
                column: "TechniqueId1",
                principalTable: "Techniques",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScenarioSteps_Scenarios_ScenarioId",
                table: "ScenarioSteps");

            migrationBuilder.DropForeignKey(
                name: "FK_ScenarioSteps_Scenarios_ScenarioId1",
                table: "ScenarioSteps");

            migrationBuilder.DropForeignKey(
                name: "FK_ScenarioSteps_Techniques_TechniqueId1",
                table: "ScenarioSteps");

            migrationBuilder.DropTable(
                name: "AnswerOptions");

            migrationBuilder.DropTable(
                name: "Scenarios");

            migrationBuilder.DropTable(
                name: "QuizQuestions");

            migrationBuilder.DropIndex(
                name: "IX_ScenarioSteps_ScenarioId",
                table: "ScenarioSteps");

            migrationBuilder.DropIndex(
                name: "IX_ScenarioSteps_ScenarioId1",
                table: "ScenarioSteps");

            migrationBuilder.DropIndex(
                name: "IX_ScenarioSteps_TechniqueId1",
                table: "ScenarioSteps");

            migrationBuilder.DropColumn(
                name: "ScenarioId",
                table: "ScenarioSteps");

            migrationBuilder.DropColumn(
                name: "ScenarioId1",
                table: "ScenarioSteps");

            migrationBuilder.DropColumn(
                name: "TechniqueId1",
                table: "ScenarioSteps");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Techniques",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ThumbnailUrl",
                table: "Techniques",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Techniques",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ModelUrl",
                table: "Techniques",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Difficulty",
                table: "Techniques",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Techniques",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TechniqueId",
                table: "ScenarioSteps",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Instruction",
                table: "ScenarioSteps",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "ScenarioSteps",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExpectedAction",
                table: "ScenarioSteps",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
