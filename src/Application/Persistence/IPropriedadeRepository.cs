using Domain.Entities;

namespace Application.Persistence;

public interface IPropriedadeRepository : IRepository<Propriedade>
{
    Task<Propriedade?> ObterComTalhoesAsync(Guid id);
    Task<IEnumerable<Propriedade>> ObterDoProprietarioAsync(Guid proprietarioId);
}
