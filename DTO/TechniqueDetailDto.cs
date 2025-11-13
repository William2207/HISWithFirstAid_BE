namespace FirstAidAPI.DTO
{
    public class TechniqueDetailDto : TechniqueDto
    {
        public List<QuizQuestionDto> QuizQuestions { get; set; } = new();
        public List<TechniqueStepDto> TechniqueSteps { get; set; } = new();
    }
}