using Application.Persistence;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public sealed class PropriedadeRepository : Repository<Propriedade>, IPropriedadeRepository
{
    public PropriedadeRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Propriedade?> ObterComTalhoesAsync(Guid id, CancellationToken cancellation = default) =>
        await _dbSet.Include(p => p.Talhoes).SingleOrDefaultAsync(p => p.Id == id, cancellation);

    public async Task<IEnumerable<Propriedade>> ObterDoProprietarioAsync(Guid proprietarioId, CancellationToken cancellation = default) =>
        await _dbSet.Where(p => p.ProprietarioId == proprietarioId).ToArrayAsync(cancellation);
}
