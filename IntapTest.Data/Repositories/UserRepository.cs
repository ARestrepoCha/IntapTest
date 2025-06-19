using IntapTest.Data.Base;
using IntapTest.Data.Entities;
using IntapTest.Data.Repositories.Interfaces;

namespace IntapTest.Data.Repositories
{
    public class UserRepository : BaseRepository<User, IntapTestDbContext>, IUserRepository
    {
        public UserRepository(IRequestInfo<IntapTestDbContext> RequestInfo) : base(RequestInfo)
        {
        }
    }
}
