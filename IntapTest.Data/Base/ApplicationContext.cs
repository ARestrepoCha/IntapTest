using Microsoft.AspNetCore.Http;

namespace IntapTest.Data.Base
{
    public class ApplicationContext
    {
        protected ApplicationContext() { }

        public static string GetHttpContextSessionItem(IHttpContextAccessor httpContextManager, string keyName)
        {
            return httpContextManager?.HttpContext?.User?.FindFirst(keyName)?.Value ?? null;
        }

        public static bool UserIsInRole(IHttpContextAccessor httpContextManager, string role)
        {
            return httpContextManager?.HttpContext?.User?.IsInRole(role) ?? false;
        }
    }
}
