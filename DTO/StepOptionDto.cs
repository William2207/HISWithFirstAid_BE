namespace FirstAidAPI.DTO
{
    public class StepOptionDto
    {
        public int Id { get; set; }
        public string OptionKey { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}