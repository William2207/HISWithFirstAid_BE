using FirstAidAPI.DTO;
using FirstAidAPI.Models;
using FirstAidAPI.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstAidAPI.Service.Implement
{
    public class ScenarioService : IScenarioService
    {
        private readonly IScenarioRepository _scenarioRepository;

        public ScenarioService(IScenarioRepository scenarioRepository)
        {
            _scenarioRepository = scenarioRepository;
        }

        public async Task<IEnumerable<Scenario>> GetAllScenariosAsync()
        {
            return await _scenarioRepository.GetAllAsync();
        }

        public async Task<PagedResult<Scenario>> GetScenariosAsync(int page, int pageSize, List<string>? difficulties, List<string>? types, string? search)
        {
            // Chuyển logic validation từ Controller vào đây
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 9;
            if (pageSize > 100) pageSize = 100;

            return await _scenarioRepository.GetAllFilteredAndPagedAsync(page, pageSize, difficulties, types, search);
        }

        public async Task<Scenario?> GetScenarioByIdAsync(int id)
        {
            return await _scenarioRepository.GetByIdAsync(id);
        }

        public async Task<Scenario> CreateScenarioAsync(Scenario scenario)
        {
            // Nơi để thêm business logic, ví dụ:
            // - Kiểm tra xem tên Scenario đã tồn tại chưa.
            // - Đặt IsPublished = false cho kịch bản mới tạo để chờ duyệt.
            await _scenarioRepository.AddAsync(scenario);
            await _scenarioRepository.SaveChangesAsync();
            return scenario;
        }

        public async Task<bool> UpdateScenarioAsync(int id, Scenario scenario)
        {
            var existingScenario = await _scenarioRepository.GetByIdAsync(id);
            if (existingScenario == null)
            {
                return false;
            }

            // Ánh xạ các thuộc tính có thể thay đổi
            existingScenario.Name = scenario.Name;
            existingScenario.Title = scenario.Title;
            existingScenario.Description = scenario.Description;
            existingScenario.Type = scenario.Type;
            existingScenario.Difficulty = scenario.Difficulty;
            existingScenario.Duration = scenario.Duration;
            existingScenario.Icon = scenario.Icon;
            existingScenario.PassingScore = scenario.PassingScore;
            existingScenario.IsPublished = scenario.IsPublished;

            _scenarioRepository.Update(existingScenario);
            return await _scenarioRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteScenarioAsync(int id)
        {
            var scenarioToDelete = await _scenarioRepository.GetByIdAsync(id);
            if (scenarioToDelete == null)
            {
                return false;
            }

            _scenarioRepository.Delete(scenarioToDelete);
            return await _scenarioRepository.SaveChangesAsync();
        }
    }
}