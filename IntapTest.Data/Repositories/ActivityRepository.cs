using IntapTest.Data.Base;
using IntapTest.Data.Entities;
using IntapTest.Data.Repositories.Interfaces;

namespace IntapTest.Data.Repositories
{
    public class ActivityRepository : BaseRepository<Activity, IntapTestDbContext>, IActivityRepository
    {
        public ActivityRepository(IRequestInfo<IntapTestDbContext> RequestInfo) : base(RequestInfo)
        {
        }
    }
}
