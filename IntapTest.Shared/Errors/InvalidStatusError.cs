using FluentResults;

namespace IntapTest.Shared.Errors
{
    public class InvalidStatusError : Error
    {
        public InvalidStatusError() { }
        public InvalidStatusError(string message) : base(message) { }
    }
}
