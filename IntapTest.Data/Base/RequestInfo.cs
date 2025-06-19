using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IntapTest.Data.Base
{
    public class RequestInfo<TDbContext> : IRequestInfo<TDbContext> where TDbContext : DbContext
    {
        private readonly IServiceScope Scope;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly string strUserId;
        private readonly string strRole;

        public RequestInfo(IServiceProvider serviceProvider, IHttpContextAccessor _contextAccessor)
        {
            contextAccessor = _contextAccessor;
            Scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            strUserId = ApplicationContext.GetHttpContextSessionItem(contextAccessor, "id");
            strRole = ApplicationContext.GetHttpContextSessionItem(contextAccessor, "role");

        }
        public Guid UserId => string.IsNullOrEmpty(strUserId) ? Guid.Empty : Guid.Parse(strUserId);
        public string Role => string.IsNullOrEmpty(strRole) ? string.Empty : strRole;
        public TDbContext Context => Scope.ServiceProvider.GetRequiredService<TDbContext>();
        bool IRequestInfo<TDbContext>.IsInRole(string role) => ApplicationContext.UserIsInRole(contextAccessor, role);
    }
}
