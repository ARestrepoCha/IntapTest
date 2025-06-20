using FluentResults;
using IntapTest.Shared.Dtos.Requests;
using IntapTest.Shared.Dtos.Responses;

namespace IntapTest.Domain.Services.Activities
{
    public interface IActivityService
    {
        Task<Result<bool>> CreateActivity(CreateActivityRequestDto createActivityRequest);
        Task<Result<bool>> UpdateActivity(Guid activityId, UpdateActivityRequestDto updateActivityRequest);
        Task<Result<bool>> DeleteActivity(Guid activityId);
        Task<Result<List<ActivityResponseDto>>> GetActivities();
    }
}
