using Application.DTOs;

namespace Application.UseCases.Talhoes.ObterTalhoesDaPropriedade;

public interface IObterTalhoesDaPropriedadeUseCase : IUseCase<ObterTalhoesDaPropriedadeDTO, IEnumerable<TalhaoDTO>>
{
}
