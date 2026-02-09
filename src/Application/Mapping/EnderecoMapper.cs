using Application.DTOs;
using Domain.ValueObjects;

namespace Application.Mapping;

internal static class EnderecoMapper
{
    public static Endereco ToValueObject(this EnderecoDTO dto) =>
        new Endereco
        {
            Logradouro = dto.Logradouro,
            Numero = dto.Numero,
            Complemento = dto.Complemento,
            Bairro = dto.Bairro,
            Cidade = dto.Cidade,
            UF = dto.UF,
            CEP = dto.CEP
        };

    public static EnderecoDTO ToDTO(this Endereco endereco) =>
        new EnderecoDTO
        {
            Logradouro = endereco.Logradouro,
            Numero = endereco.Numero,
            Complemento = endereco.Complemento,
            Bairro = endereco.Bairro,
            Cidade = endereco.Cidade,
            UF = endereco.UF,
            CEP = endereco.CEP
        };
}
