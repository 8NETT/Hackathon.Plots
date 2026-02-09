using Application.DTOs;
using Domain.Entities;

namespace Application.Mapping;

internal static class PropriedadeMapper
{
    public static Propriedade ToEntity(this CadastrarPropriedadeDTO dto) =>
        Propriedade.Nova
            .ProprietarioId(dto.ProprietarioId)
            .Nome(dto.Nome)
            .Descricao(dto.Descricao)
            .Endereco(dto.Endereco?.ToValueObject())
            .Build();

    public static PropriedadeDTO ToDTO(this Propriedade propriedade) =>
        new PropriedadeDTO
        {
            Id = propriedade.Id,
            ProprietarioId = propriedade.ProprietarioId,
            Nome = propriedade.Nome,
            Descricao = propriedade.Descricao,
            Endereco = propriedade.Endereco?.ToDTO()
        };
}
