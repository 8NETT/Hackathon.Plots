using Application.DTOs;
using Domain.Entities;

namespace Application.Mapping;

internal static class TalhaoMapper
{
    public static TalhaoDTO ToDTO(this Talhao talhao) => new()
    {
        Id = talhao.Id,
        PropriedadeId = talhao.PropriedadeId,
        Nome = talhao.Nome,
        Descricao = talhao.Descricao,
        Coordenadas = talhao.Coordenadas.ToDTO(),
        Area = talhao.Area
    };
}
