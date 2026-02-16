using Application.DTOs;

namespace Application.UseCases.Propriedades.ObterPropriedadesDoProprietario;

public interface IObterPropriedadesDoProprietarioUseCase : IUseCase<Guid, IEnumerable<PropriedadeDTO>>
{
}
