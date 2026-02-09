using Application.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public abstract class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected ApplicationDbContext _context;
    protected DbSet<T> _dbSet;

    protected internal Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<T?> ObterAsync(Guid id) =>
        await _dbSet.SingleOrDefaultAsync(e => e.Id == id);

    public void Cadastrar(T entity) =>
        _dbSet.Add(entity);

    public void Atualizar(T entity) =>
        _dbSet.Update(entity);

    public void Remover(T entity) =>
        _dbSet.Remove(entity);
}
