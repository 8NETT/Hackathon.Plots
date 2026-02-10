namespace Application.Persistence;

public interface IUnitOfWork : IDisposable
{
    IPropriedadeRepository PropriedadeRepository { get; }
    ITalhaoRepository TalhaoRepository { get; }
    Task CommitAsync();
    Task CommitAsync(CancellationToken cancellationToken = default);
}
