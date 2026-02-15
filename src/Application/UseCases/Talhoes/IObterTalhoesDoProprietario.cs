using Application.DTOs;

namespace Application.UseCases.Talhoes;

public interface IObterTalhoesDoProprietario : IUseCase<Guid, IEnumerable<TalhaoDTO>>
{
}
