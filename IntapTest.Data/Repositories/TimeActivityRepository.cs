using IntapTest.Data.Base;
using IntapTest.Data.Entities;
using IntapTest.Data.Repositories.Interfaces;

namespace IntapTest.Data.Repositories
{
    public class TimeActivityRepository : BaseRepository<TimeActivity, IntapTestDbContext>, ITimeActivityRepository
    {
        public TimeActivityRepository(IRequestInfo<IntapTestDbContext> RequestInfo) : base(RequestInfo)
        {
        }
    }
}
