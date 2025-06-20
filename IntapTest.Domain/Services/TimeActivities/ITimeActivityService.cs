using FluentResults;
using IntapTest.Shared.Dtos.Requests;
using IntapTest.Shared.Dtos.Responses;

namespace IntapTest.Domain.Services.TimeActivities
{
    public interface ITimeActivityService
    {
        Task<Result<bool>> CreateTimeActivity(CreateTimeActivityRequestDto createTimeActivityRequest);
        Task<Result<bool>> UpdateTimeActivity(Guid timeActivityId, UpdateTimeActivityRequestDto updateTimeActivityRequest);
        Task<Result<bool>> DeleteTimeActivity(Guid timeActivityId);
        Task<Result<List<TimeActivityResponseDto>>> GetTimeActivities(Guid activityId);
    }
}
