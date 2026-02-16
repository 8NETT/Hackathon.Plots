using API.Requests;
using Application.DTOs;
using Application.UseCases.Talhoes.AlterarTalhao;
using Application.UseCases.Talhoes.CadastrarTalhao;

namespace API.Mapping;

internal static class TalhaoMapper
{
    public static CadastrarTalhaoDTO ToApplicationDTO(this CadastrarTalhaoRequest request, Guid usuarioId) => new()
    {
        UsuarioId = usuarioId,
        PropriedadeId = request.PropriedadeId!.Value,
        Nome = request.Nome!,
        Descricao = request.Descricao,
        Coordenadas = request.Coordenadas!.ToApplicationDTO(),
        Area = request.Area!.Value
    };

    public static AlterarTalhaoDTO ToApplicationDTO(this AlterarTalhaoRequest request, Guid id, Guid usuarioId) => new()
    {
        Id = id,
        UsuarioId = usuarioId,
        Nome = request.Nome!,
        Descricao = request.Descricao,
        Coordenadas = request.Coordenadas!.ToApplicationDTO(),
        Area = request.Area!.Value
    };

    public static CoordenadasDTO ToApplicationDTO(this CoordenadasRequest request) => new()
    {
        Latitude = request.Latitude!.Value,
        Longitude = request.Longitude!.Value
    };
}
