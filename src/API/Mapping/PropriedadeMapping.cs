using API.Requests;
using Application.DTOs;
using Application.UseCases.Propriedades.AlterarPropriedade;
using Application.UseCases.Propriedades.CadastrarPropriedade;

namespace API.Mapping;

internal static class PropriedadeMapping
{
    public static CadastrarPropriedadeDTO ToApplicationDTO(this CadastrarPropriedadeRequest request, Guid usuarioId) =>
        new CadastrarPropriedadeDTO
        {
            UsuarioId = usuarioId,
            Nome = request.Nome!,
            Descricao = request.Descricao,
            Endereco = request.Endereco?.ToApplicationDTO()
        };

    public static AlterarPropriedadeDTO ToApplicationDTO(this AlterarPropriedadeRequest request, Guid id, Guid usuarioId) =>
        new AlterarPropriedadeDTO
        {
            Id = id,
            UsuarioId = usuarioId,
            Nome = request.Nome!,
            Descricao = request.Descricao,
            Endereco = request.Endereco?.ToApplicationDTO()
        };

    public static EnderecoDTO ToApplicationDTO(this EnderecoRequest request) =>
        new EnderecoDTO
        {
            Logradouro = request.Logradouro!,
            Numero = request.Numero!,
            Complemento = request.Complemento,
            Bairro = request.Bairro,
            Cidade = request.Cidade!,
            UF = request.UF!,
            CEP = request.CEP
        };
}
