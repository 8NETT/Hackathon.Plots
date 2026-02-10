using Application.Persistence;
using Infrastructure.Persistence.Repositories;

namespace Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private ApplicationDbContext _context;
    private IPropriedadeRepository _propriedadeRepository = null!;
    private ITalhaoRepository _talhaoRepository = null!;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IPropriedadeRepository PropriedadeRepository =>
        _propriedadeRepository = _propriedadeRepository ?? new PropriedadeRepository(_context);

    public ITalhaoRepository TalhaoRepository => 
        _talhaoRepository = _talhaoRepository ?? new TalhaoRepository(_context);

    public async Task CommitAsync() =>
        await _context.SaveChangesAsync();

    public async Task CommitAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);

    public void Dispose() => _context.Dispose();
}
