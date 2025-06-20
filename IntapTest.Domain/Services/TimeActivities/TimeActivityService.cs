using FluentResults;
using IntapTest.Data;
using IntapTest.Data.Base;
using IntapTest.Data.Entities;
using IntapTest.Data.Repositories.Interfaces;
using IntapTest.Shared.Dtos.Requests;
using IntapTest.Shared.Dtos.Responses;
using IntapTest.Shared.Errors;
using Mapster;

namespace IntapTest.Domain.Services.TimeActivities
{
    public class TimeActivityService : ITimeActivityService
    {
        private readonly IRequestInfo<IntapTestDbContext> _requestInfo;
        private readonly IUserRepository _userRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly ITimeActivityRepository _timeActivityRepository;

        public TimeActivityService(IRequestInfo<IntapTestDbContext> requestInfo,
            IUserRepository userRepository,
            IActivityRepository activityRepository,
            ITimeActivityRepository timeActivityRepository)
        {
            _requestInfo = requestInfo;
            _userRepository = userRepository;
            _activityRepository = activityRepository;
            _timeActivityRepository = timeActivityRepository;
        }

        public async Task<Result<bool>> CreateTimeActivity(CreateTimeActivityRequestDto createTimeActivityRequest)
        {
            var user = await _userRepository.GetById(_requestInfo.UserId);
            if (user == null)
                return Result.Fail(new ResourceNotFoundError("Usuario no encontrado."));

            var activity = await _activityRepository.GetById(createTimeActivityRequest.ActivityId);
            if (activity == null)
                return Result.Fail(new ResourceNotFoundError("Actividad no encontrada."));

            var timeActivitites = await _timeActivityRepository.Query(x => x.ActivityId == activity.Id && x.IsActive && !x.IsDeleted);
            var hours = timeActivitites.Sum(x => x.Hours);
            if ((hours + createTimeActivityRequest.Hours) > 8)
                return Result.Fail(new InvalidInputError("El tiempo no puede sumar más de 8 horas."));

            var entity = createTimeActivityRequest.Adapt<TimeActivity>();
            entity.ActivityId = activity.Id;

            await _timeActivityRepository.Create(entity);
            return Result.Ok(true);
        }

        public async Task<Result<bool>> DeleteTimeActivity(Guid timeActivityId)
        {
            var user = await _userRepository.GetById(_requestInfo.UserId);
            if (user == null)
                return Result.Fail(new ResourceNotFoundError("Usuario no encontrado."));

            var timeActivity = await _timeActivityRepository.GetById(timeActivityId);
            if (timeActivity == null)
                return Result.Fail(new ResourceNotFoundError("Actividad no encontrada."));

            await _timeActivityRepository.Remove(timeActivity);
            return Result.Ok(true);
        }

        public async Task<Result<List<TimeActivityResponseDto>>> GetTimeActivities(Guid activityId)
        {
            var user = await _userRepository.GetById(_requestInfo.UserId);
            if (user == null)
                return Result.Fail(new ResourceNotFoundError("Usuario no encontrado."));

            var activity = await _activityRepository.GetById(activityId);
            if (activity == null)
                return Result.Fail(new ResourceNotFoundError("Actividad no encontrada."));

            var timeActivities = await _timeActivityRepository.GetByFilter(x => x.ActivityId == activity.Id && x.IsActive && !x.IsDeleted);
            return Result.Ok(timeActivities.Adapt<List<TimeActivityResponseDto>>());
        }

        public async Task<Result<bool>> UpdateTimeActivity(Guid timeActivityId, UpdateTimeActivityRequestDto updateTimeActivityRequest)
        {
            var user = await _userRepository.GetById(_requestInfo.UserId);
            if (user == null)
                return Result.Fail(new ResourceNotFoundError("Usuario no encontrado."));

            var timeActivity = await _timeActivityRepository.GetById(timeActivityId);
            if (timeActivity == null)
                return Result.Fail(new ResourceNotFoundError("Actividad no encontrada."));

            timeActivity = updateTimeActivityRequest.Adapt(timeActivity);

            await _timeActivityRepository.Update(timeActivity);
            return Result.Ok(true);
        }
    }
}
