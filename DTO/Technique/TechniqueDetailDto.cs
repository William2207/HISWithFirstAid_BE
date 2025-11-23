using FirstAidAPI.DTO.Quiz;

namespace FirstAidAPI.DTO.Technique
{
    public class TechniqueDetailDto : TechniqueDto
    {
        public List<QuizQuestionDto> QuizQuestions { get; set; } = new();
        public List<TechniqueStepDto> TechniqueSteps { get; set; } = new();
    }
}