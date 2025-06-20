using FluentResults;

namespace IntapTest.Shared.Errors
{
    public class ForbiddenError : Error
    {
        public ForbiddenError() { }
        public ForbiddenError(string message) : base(message) { }
    }
}
