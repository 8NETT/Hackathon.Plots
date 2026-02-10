using Application.Persistence;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;

namespace Application.UseCases;

public abstract class ResultUseCase<TInput, TOutput> : BaseUseCase<TInput, Result<TOutput>>
{
    protected IValidator<TInput>? _validator; 

    protected ResultUseCase(IUnitOfWork unitOfWork, IValidator<TInput>? validator) : base(unitOfWork)
    {
        _validator = validator;
    }

    public override sealed async Task<Result<TOutput>> HandleAsync(TInput input, CancellationToken cancellationToken = default)
    {
        var validation = _validator?.Validate(input);
        if (validation is not null && !validation.IsValid)
            return Result.Invalid(validation.AsErrors());

        return await ExecuteCoreAsync(input);
    }

    protected abstract Task<Result<TOutput>> ExecuteCoreAsync(TInput input, CancellationToken cancellationToken = default);
}
