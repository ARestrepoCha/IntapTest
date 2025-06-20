using IntapTest.Domain.Services.TimeActivities;
using IntapTest.Shared.Dtos.Requests;
using IntapTest.Shared.Enums;
using IntapTest.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntapTest.Api.Controllers
{
    public class TimeActivityController : BaseController
    {
        private readonly ITimeActivityService _timeActivityService;

        public TimeActivityController(ITimeActivityService timeActivityService)
        {
            _timeActivityService = timeActivityService;
        }

        [HttpPost]
        [Authorize(Roles = nameof(RoleEnum.Employee))]
        public async Task<IActionResult> CreateTimeActivity([FromBody] CreateTimeActivityRequestDto createTimeActivityRequest)
        {
            var result = await _timeActivityService.CreateTimeActivity(createTimeActivityRequest);
            return !result.IsFailed ? Ok(result.Value) : result.GetErrorResponse();
        }

        [HttpPut]
        [Route("Update/{id}")]
        [Authorize(Roles = nameof(RoleEnum.Employee))]
        public async Task<IActionResult> UpdateTimeActivity([FromRoute] Guid id, UpdateTimeActivityRequestDto updateTimeActivityRequest)
        {
            var result = await _timeActivityService.UpdateTimeActivity(id, updateTimeActivityRequest);
            return !result.IsFailed ? Ok(result.Value) : result.GetErrorResponse();
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        [Authorize(Roles = nameof(RoleEnum.Employee))]
        public async Task<IActionResult> DeleteTimeActivity([FromRoute] Guid id)
        {
            var result = await _timeActivityService.DeleteTimeActivity(id);
            return !result.IsFailed ? Ok(result.Value) : result.GetErrorResponse();
        }

        [HttpGet]
        [Route("GetTimeActivities/{activityId}")]
        [Authorize(Roles = nameof(RoleEnum.Employee))]
        public async Task<IActionResult> GetTimeActivitites([FromRoute] Guid activityId)
        {
            var result = await _timeActivityService.GetTimeActivities(activityId);
            return !result.IsFailed ? Ok(result.Value) : result.GetErrorResponse();
        }
    }
}
