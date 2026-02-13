namespace Application.UseCases;

public interface IUseCase<in TInput, TOutput>
{
    Task<Result<TOutput>> HandleAsync(TInput input, CancellationToken cancellationToken);
}
