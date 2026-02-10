namespace Application.UseCases;

public interface IUseCase<in TInput, TOutput>
{
    Task<TOutput> HandleAsync(TInput input, CancellationToken cancellationToken);
}
