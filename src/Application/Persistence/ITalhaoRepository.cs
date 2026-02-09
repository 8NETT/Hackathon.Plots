using Domain.Entities;

namespace Application.Persistence;

public interface ITalhaoRepository : IRepository<Talhao>
{
    Task<IEnumerable<Talhao>> ObterDaPropriedade(Guid propriedadeId);
}
