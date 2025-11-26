using System;

namespace Case.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public string ErrorCode { get; }

        protected DomainException()
        {
        }

        protected DomainException(string message) : base(message)
        {
        }

        protected DomainException(string message, Exception inner) : base(message, inner)
        {
        }

        public DomainException(string errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}