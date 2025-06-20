using IntapTest.Data.Base;
using IntapTest.Data.Entities;
using IntapTest.Data.Repositories.Interfaces;
using IntapTest.Shared.Dtos;
using IntapTest.Shared.Dtos.Responses;
using Microsoft.EntityFrameworkCore;

namespace IntapTest.Data.Repositories
{
    public class ActivityRepository : BaseRepository<Activity, IntapTestDbContext>, IActivityRepository
    {
        public ActivityRepository(IRequestInfo<IntapTestDbContext> RequestInfo) : base(RequestInfo)
        {
        }

        public async Task<List<ActivityResponseDto>> GetActivities(Guid userId)
        {
            var data = await _requestInfo.Context.Activities
                .Where(x => x.UserId == userId && x.IsActive && !x.IsDeleted)
                .Select(a => new ActivityResponseDto
                {
                    Id = a.Id,
                    Descripcion = a.Descripcion,
                    TimeActivities = a.TimeActivities.Select(t => new TimeActivityDto
                    {
                        Date = t.Date,
                        Hours = t.Hours
                    }).ToList()
                })
                .ToListAsync();

            return data;
        }
    }
}
