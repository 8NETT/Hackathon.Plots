using Domain.Entities;

namespace Application.Persistence;

public interface IPropriedadeRepository : IRepository<Propriedade>
{
    Task<Propriedade?> ObterComTalhoesAsync(Guid id, CancellationToken cancellation = default);
    Task<IEnumerable<Propriedade>> ObterDoProprietarioAsync(Guid proprietarioId, CancellationToken cancellation = default);
}
