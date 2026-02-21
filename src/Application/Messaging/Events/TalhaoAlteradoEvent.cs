using Application.DTOs;
using Application.Mapping;
using Domain.Entities;

namespace Application.Messaging.Events;

public sealed record TalhaoAlteradoEvent : Event
{
    public Guid TalhaoId { get; }
    public Guid PropriedadeId { get; }
    public Guid ProprietarioId { get; }
    public string Nome { get; }
    public string? Descricao { get; }
    public CoordenadasDTO Coordenadas { get; }
    public decimal Area { get; }

    public TalhaoAlteradoEvent(Talhao talhao, Propriedade propriedade) : base("TalhaoAlterado")
    {
        TalhaoId = talhao.Id;
        PropriedadeId = propriedade.Id;
        ProprietarioId = propriedade.ProprietarioId;
        Nome = talhao.Nome;
        Descricao = talhao.Descricao;
        Coordenadas = talhao.Coordenadas.ToDTO();
        Area = talhao.Area;
    }
}
