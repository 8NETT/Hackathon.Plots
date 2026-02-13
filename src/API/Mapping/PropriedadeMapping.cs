using API.Requests;
using Application.DTOs;

namespace API.Mapping;

internal static class PropriedadeMapping
{
    public static CadastrarPropriedadeDTO ToApplicationDTO(this CadastrarPropriedadeRequest request, Guid usuarioId) =>
        new CadastrarPropriedadeDTO
        {
            ProprietarioId = usuarioId,
            Nome = request.Nome!,
            Descricao = request.Descricao,
            Endereco = request.Endereco?.ToApplicationDTO()
        };

    public static EnderecoDTO ToApplicationDTO(this CadastrarEnderecoRequest request) =>
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
