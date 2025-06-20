using FluentResults;

namespace IntapTest.Shared.Errors
{
    public class UnexpectedError : Error
    {
        public UnexpectedError() { }
        public UnexpectedError(string message) : base(message) { }
    }
}
