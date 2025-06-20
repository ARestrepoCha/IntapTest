using IntapTest.Data.Entities;
using IntapTest.Shared.Dtos.Responses;

namespace IntapTest.Data.Repositories.Interfaces
{
    public interface IActivityRepository : IBaseRepository<Activity>
    {
        Task<List<ActivityResponseDto>> GetActivities(Guid userId);
    }
}
