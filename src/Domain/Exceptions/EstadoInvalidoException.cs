namespace Domain.Exceptions;

public sealed class EstadoInvalidoException : DomainException
{
    internal EstadoInvalidoException(string message) : base(message) { }
}
