using Application.DTOs;

namespace Application.UseCases.Talhoes.ObterTalhoesDoProprietario;

public interface IObterTalhoesDoProprietarioUseCase : IUseCase<Guid, IEnumerable<TalhaoDTO>>
{
}
