using Domain.Entities;

namespace Application.Persistence;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> ObterAsync(Guid id);
    void Cadastrar(T entity);
    void Atualizar(T entity);
    void Remover(T entity);
}
