using Domain.Entities;

namespace Application.Persistence;

public interface ITalhaoRepository : IRepository<Talhao>
{
    Task<Talhao?> ObterComPropriedade(Guid id);
    Task<IEnumerable<Talhao>> ObterDaPropriedade(Guid propriedadeId);
    Task<IEnumerable<Talhao>> ObterDoProprietario(Guid proprietarioId);
}
