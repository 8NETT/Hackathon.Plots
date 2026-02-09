namespace Domain.Exceptions;

public abstract class DomainException : Exception
{
    protected internal DomainException(string message) : base(message) { }
}
