using Application.DTOs;
using Application.Mapping;
using Application.Persistence;

namespace Application.UseCases.Propriedades;

public sealed class ObterPropriedadeUseCase : BaseUseCase<Guid, PropriedadeDTO>, IObterPropriedadeUseCase
{
    public ObterPropriedadeUseCase(IUnitOfWork unitOfWork, IValidator<Guid>? validator, ILogger logger)
        : base(unitOfWork, validator, logger) { }

    protected override async Task<Result<PropriedadeDTO>> ExecuteCoreAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var propriedade = await _unitOfWork.PropriedadeRepository.ObterAsync(id);

        if (propriedade is null)
            return Result.NotFound("Propriedade não localizada.");

        return propriedade.ToDTO();
    }
}
