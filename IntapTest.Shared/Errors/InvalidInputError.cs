using FluentResults;

namespace IntapTest.Shared.Errors
{
    public class InvalidInputError : Error
    {
        public InvalidInputError() { }
        public InvalidInputError(string message) : base(message) { }
    }
}
