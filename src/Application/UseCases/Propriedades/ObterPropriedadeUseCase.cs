using Application.DTOs;
using Application.Mapping;
using Application.Persistence;

namespace Application.UseCases.Propriedades;

public sealed class ObterPropriedadeUseCase : BaseUseCase<ObterPropriedadeDTO, PropriedadeDTO>, IObterPropriedadeUseCase
{
    public ObterPropriedadeUseCase(IUnitOfWork unitOfWork, ILogger logger)
        : base(unitOfWork, null, logger) { }

    protected override async Task<Result<PropriedadeDTO>> ExecuteCoreAsync(ObterPropriedadeDTO dto, CancellationToken cancellationToken = default)
    {
        var propriedade = await _unitOfWork.PropriedadeRepository.ObterAsync(dto.Id);

        if (propriedade is null)
            return Result.NotFound("Propriedade não localizada.");
        if (propriedade.ProprietarioId != dto.UsuarioId)
            return Result.Forbidden("Usuário não é o proprietário da propriedade.");

        return propriedade.ToDTO();
    }
}
