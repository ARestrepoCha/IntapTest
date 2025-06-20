using FluentResults;
using IntapTest.Shared.Errors;
using IntapTest.Shared.Utils;
using Microsoft.AspNetCore.Mvc;

namespace IntapTest.Shared.Extensions
{
    public static class WebApiResponses
    {
        public static ActionResult GetErrorResponse<T>(this Result<T> result)
        {
            if (result.HasError<InvalidInputError>() || result.HasError<InvalidStatusError>())
                return new BadRequestObjectResult(result.Reasons);

            if (result.HasError<ResourceNotFoundError>())
                return new NotFoundObjectResult(result.Reasons);

            if (result.HasError<ResourceConflictError>())
                return new ConflictObjectResult(result.Reasons);

            if (result.HasError<ForbiddenError>())
                return new ForbiddenErrorResult(result.Reasons);

            return new InternalServerErrorResult(result.Reasons);
        }
    }
}
