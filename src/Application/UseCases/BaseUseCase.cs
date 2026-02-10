using Application.Persistence;

namespace Application.UseCases;

public abstract class BaseUseCase<TInput, TOuput> : IUseCase<TInput, TOuput>
{
    protected IUnitOfWork _unitOfWork;

    protected BaseUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public abstract Task<TOuput> HandleAsync(TInput input, CancellationToken cancellationToken = default);
}
