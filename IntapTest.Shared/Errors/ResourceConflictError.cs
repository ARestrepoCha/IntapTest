using FluentResults;

namespace IntapTest.Shared.Errors
{
    public class ResourceConflictError : Error
    {
        public ResourceConflictError() { }
        public ResourceConflictError(string message) : base(message) { }
    }
}
