using Application.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class TalhaoRepository : Repository<Talhao>, ITalhaoRepository
{
    public TalhaoRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Talhao>> ObterDaPropriedade(Guid propriedadeId) =>
        await _dbSet.Where(t => t.PropriedadeId == propriedadeId).ToArrayAsync();
}
