using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntapTest.Shared.Utils
{
    public class ForbiddenErrorResult : ObjectResult
    {
        public ForbiddenErrorResult(object value) : base(value)
        {
            StatusCode = StatusCodes.Status403Forbidden;
        }
    }
}
