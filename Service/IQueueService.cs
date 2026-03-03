using FirstAidAPI.DTO.Queue;

namespace FirstAidAPI.Service
{
    public interface IQueueService
    {
        Task<QueueDTO> IssueQueueAsync();

        Task<QueueDTO> CallNextQueueAsync(int receptionistId);

        Task<PrintTicketDto> CompleteQueueAsync(int queueId, CompleteQueueRequest request);
    }
}
