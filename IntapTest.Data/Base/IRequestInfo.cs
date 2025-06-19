using Microsoft.EntityFrameworkCore;

namespace IntapTest.Data.Base
{
    public interface IRequestInfo<TDbContext>
        where TDbContext : DbContext
    {
        Guid UserId { get; }
        string Role { get; }
        bool IsInRole(string role);

        TDbContext Context { get; }
    }
}
