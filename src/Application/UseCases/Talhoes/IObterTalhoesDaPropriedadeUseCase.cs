using Application.DTOs;

namespace Application.UseCases.Talhoes;

public interface IObterTalhoesDaPropriedadeUseCase : IUseCase<ObterTalhoesDaPropriedadeDTO, IEnumerable<TalhaoDTO>>
{
}
