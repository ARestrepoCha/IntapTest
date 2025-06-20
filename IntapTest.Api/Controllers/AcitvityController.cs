using FluentResults;
using IntapTest.Domain.Services.Activities;
using IntapTest.Shared.Dtos.Requests;
using IntapTest.Shared.Enums;
using IntapTest.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntapTest.Api.Controllers
{
    public class AcitvityController : BaseController
    {
        private readonly IActivityService _activityService;

        public AcitvityController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        [HttpPost]
        [Authorize(Roles = nameof(RoleEnum.Employee))]
        public async Task<ActionResult<Result<bool>>> CreateActivity([FromBody] CreateActivityRequestDto createActivityRequest)
        {
            var result = await _activityService.CreateActivity(createActivityRequest);
            return !result.IsFailed ? Ok(result.Value) : result.GetErrorResponse();
        }

        [HttpPut]
        [Route("Update/{id}")]
        [Authorize(Roles = nameof(RoleEnum.Employee))]
        public async Task<ActionResult<Result<bool>>> UpdateActivity([FromRoute] Guid id, UpdateActivityRequestDto updateActivityRequest)
        {
            var result = await _activityService.UpdateActivity(id, updateActivityRequest);
            return !result.IsFailed ? Ok(result.Value) : result.GetErrorResponse();
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        [Authorize(Roles = nameof(RoleEnum.Employee))]
        public async Task<ActionResult<Result<bool>>> DeleteActivity([FromRoute] Guid id)
        {
            var result = await _activityService.DeleteActivity(id);
            return !result.IsFailed ? Ok(result.Value) : result.GetErrorResponse();
        }

        [HttpGet]
        [Authorize(Roles = nameof(RoleEnum.Employee))]
        public async Task<ActionResult<Result<bool>>> GetActivitites()
        {
            var result = await _activityService.GetActivities();
            return !result.IsFailed ? Ok(result.Value) : result.GetErrorResponse();
        }
    }
}
