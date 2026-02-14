namespace Application.DTOs;

public sealed record RemoverPropriedadeDTO
{
    public required Guid Id { get; init; }
    public required Guid UsuarioId { get; init; }
}
