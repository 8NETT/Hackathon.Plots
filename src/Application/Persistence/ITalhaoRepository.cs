using Domain.Entities;

namespace Application.Persistence;

public interface ITalhaoRepository : IRepository<Talhao>
{
    Task<Talhao?> ObterComPropriedade(Guid id, CancellationToken cancellation = default);
    Task<IEnumerable<Talhao>> ObterDoProprietario(Guid proprietarioId, CancellationToken cancellation = default);
}
