using FluentResults;
using IntapTest.Data;
using IntapTest.Data.Base;
using IntapTest.Data.Entities;
using IntapTest.Data.Repositories.Interfaces;
using IntapTest.Shared.Dtos.Requests;
using IntapTest.Shared.Dtos.Responses;
using IntapTest.Shared.Errors;
using Mapster;

namespace IntapTest.Domain.Services.Activities
{
    public class ActivityService : IActivityService
    {
        private readonly IRequestInfo<IntapTestDbContext> _requestInfo;
        private readonly IUserRepository _userRepository;
        private readonly IActivityRepository _activityRepository;

        public ActivityService(IRequestInfo<IntapTestDbContext> requestInfo,
            IUserRepository userRepository,
            IActivityRepository activityRepository)
        {
            _requestInfo = requestInfo;
            _userRepository = userRepository;
            _activityRepository = activityRepository;
        }

        public async Task<Result<bool>> CreateActivity(CreateActivityRequestDto createActivityRequest)
        {
            var user = await _userRepository.GetById(_requestInfo.UserId);
            if (user == null)
                return Result.Fail(new ResourceNotFoundError("Usuario no encontrado."));

            var entity = createActivityRequest.Adapt<Activity>();
            entity.UserId = user.Id;

            await _activityRepository.Create(entity);
            return Result.Ok(true);
        }

        public async Task<Result<bool>> DeleteActivity(Guid activityId)
        {
            var user = await _userRepository.GetById(_requestInfo.UserId);
            if (user == null)
                return Result.Fail(new ResourceNotFoundError("Usuario no encontrado."));

            var activity = await _activityRepository.GetById(activityId);
            if (activity == null)
                return Result.Fail(new ResourceNotFoundError("Actividad no encontrada."));

            await _activityRepository.Remove(activity);
            return Result.Ok(true);
        }

        public async Task<Result<List<ActivityResponseDto>>> GetActivities()
        {
            var user = await _userRepository.GetById(_requestInfo.UserId);
            if (user == null)
                return Result.Fail(new ResourceNotFoundError("Usuario no encontrado."));

            var activities = await _activityRepository.GetActivities(user.Id);
            return Result.Ok(activities);
        }

        public async Task<Result<bool>> UpdateActivity(Guid activityId, UpdateActivityRequestDto updateActivityRequest)
        {
            var user = await _userRepository.GetById(_requestInfo.UserId);
            if (user == null)
                return Result.Fail(new ResourceNotFoundError("Usuario no encontrado."));

            var activity = await _activityRepository.GetById(activityId);
            if (activity == null)
                return Result.Fail(new ResourceNotFoundError("Actividad no encontrada."));

            if (activity.UserId != user.Id)
                return Result.Fail(new InvalidInputError("No tienes permiso para actualizar esta actividad."));

            activity = updateActivityRequest.Adapt(activity);

            await _activityRepository.Update(activity);
            return Result.Ok(true);
        }
    }
}
