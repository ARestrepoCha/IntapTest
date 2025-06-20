using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntapTest.Shared.Utils
{
    public class InternalServerErrorResult : ObjectResult
    {
        public InternalServerErrorResult(object value) : base(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
