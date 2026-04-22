using FirstAidAPI.Data;
using FirstAidAPI.DTO.Queue;
using FirstAidAPI.Exceptions;
using FirstAidAPI.Models;
using FirstAidAPI.Repository;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FirstAidAPI.Service.Implement
{
    public class QueueService : IQueueService
    {
        private readonly IQueueRepository _queueRepository;
        private readonly FirstAidContext _context;

        public QueueService(IQueueRepository queueRepository, FirstAidContext context)
        {
            _queueRepository = queueRepository;
            _context = context;
        }

        public async Task<QueueDTO> GetCurrentWaitingQueue()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);
            var currentQueue = await _queueRepository.GetCurrentQueueAsync(today);
            if (currentQueue == null)
                throw new NotFoundException("Không có số nào đang chờ");
            return new QueueDTO
            {
                Id = currentQueue.Id,
                QueueNumber = currentQueue.QueueNumber,
                IssueTime = currentQueue.IssueTime
            };
        }

        public async Task<QueueDTO> IssueQueueAsync()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);
            var queue = await _queueRepository.IssueQueueAsync(today);

            return new QueueDTO
            {
                QueueNumber = queue.QueueNumber,
                IssueTime = queue.IssueTime
            };
        }

        public async Task<int> GetNextQueueNumberAsync()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);
            return await _queueRepository.GetNextQueueNumberAsync(today);
        }

        public async Task<QueueDTO> CallNextQueueAsync(int receptionistId)
        {
            using var transaction = await _context.Database
                .BeginTransactionAsync(IsolationLevel.Serializable);
            try
            {
                var nextQueue = await _queueRepository.GetNextWaitingQueueAsync();

                if (nextQueue == null)
                    throw new BusinessException("Không có số nào đang chờ");

                nextQueue.Status = "CALLED";
                nextQueue.CalledTime = DateTime.UtcNow;

                await _queueRepository.UpdateAsync(nextQueue);
                await transaction.CommitAsync();

                return new QueueDTO
                {
                    Id = nextQueue.Id,
                    QueueNumber = nextQueue.QueueNumber,
                    IssueTime = nextQueue.IssueTime,
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<PrintTicketDto> CompleteQueueAsync(int queueId, CompleteQueueRequest request)
        {
            var queue = await _queueRepository.GetByIdAsync(queueId);

            if (queue == null)
                throw new NotFoundException("Không tìm thấy số");

            // Guard tránh complete 2 lần
            if (queue.Status != "CALLED")
                throw new BusinessException($"Không thể hoàn tất số đang ở trạng thái {queue.Status}");

            queue.Status = "COMPLETED";
            queue.CompletedTime = DateTime.UtcNow;

            await _queueRepository.UpdateAsync(queue);

            return new PrintTicketDto
            {
                QueueNumber = queue.QueueNumber,
                PatientName = request.PatientName,
                Reason = request.Reason,
                Specialty = request.Specialty,
                PrintTime = DateTime.UtcNow
            };
        }
    }
}
