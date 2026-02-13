namespace Application.DTOs;

public sealed record ObterPropriedadeDTO
{
    public required Guid Id { get; set; }
    public required Guid UsuarioId { get; set; }
}
