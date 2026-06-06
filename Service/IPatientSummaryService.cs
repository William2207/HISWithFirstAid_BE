namespace FirstAidAPI.Service
{
    public interface IPatientSummaryService
    {
        Task<string> SummarizePatientAsync(int patientId);
    }
}
