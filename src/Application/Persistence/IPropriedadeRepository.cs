using Domain.Entities;

namespace Application.Persistence;

public interface IPropriedadeRepository : IRepository<Propriedade>
{
    Task<IEnumerable<Propriedade>> ObterDoProprietarioAsync(Guid proprietarioId);
}
