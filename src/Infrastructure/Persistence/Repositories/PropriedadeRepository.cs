using Application.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class PropriedadeRepository : Repository<Propriedade>, IPropriedadeRepository
{
    public PropriedadeRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Propriedade?> ObterComTalhoesAsync(Guid id) =>
        await _dbSet.Include(p => p.Talhoes).SingleOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Propriedade>> ObterDoProprietarioAsync(Guid proprietarioId) =>
        await _dbSet.Where(p => p.ProprietarioId == proprietarioId).ToArrayAsync();
}
