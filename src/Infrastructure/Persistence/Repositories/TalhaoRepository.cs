using Application.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class TalhaoRepository : Repository<Talhao>, ITalhaoRepository
{
    public TalhaoRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Talhao?> ObterComPropriedade(Guid id) =>
        await _dbSet.Include(t => t.Propriedade).SingleOrDefaultAsync(t => t.Id == id);

    public async Task<IEnumerable<Talhao>> ObterDaPropriedade(Guid propriedadeId) =>
        await _dbSet.AsNoTracking().Where(t => t.PropriedadeId == propriedadeId).ToArrayAsync();

    public async Task<IEnumerable<Talhao>> ObterDoProprietario(Guid proprietarioId) =>
        await _dbSet.AsNoTracking().Where(t => t.Propriedade.ProprietarioId == proprietarioId).ToArrayAsync();
}
