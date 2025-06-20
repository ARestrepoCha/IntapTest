using FluentResults;

namespace IntapTest.Shared.Errors
{
    public class ResourceNotFoundError : Error
    {
        public ResourceNotFoundError() { }
        public ResourceNotFoundError(string message) : base(message) { }
    }
}
