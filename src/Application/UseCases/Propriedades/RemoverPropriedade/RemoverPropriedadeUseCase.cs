using Application.DTOs;
using Application.Mapping;
using Application.Persistence;

namespace Application.UseCases.Propriedades.RemoverPropriedade;

public sealed class RemoverPropriedadeUseCase : BaseUseCase<RemoverPropriedadeDTO, PropriedadeDTO>, IRemoverPropriedadeUseCase
{
    public RemoverPropriedadeUseCase(IUnitOfWork unitOfWork, ILogger logger)
        : base(unitOfWork, logger) { }

    protected override async Task<Result<PropriedadeDTO>> ExecuteCoreAsync(RemoverPropriedadeDTO dto, CancellationToken cancellationToken = default)
    {
        var propriedade = await _unitOfWork.PropriedadeRepository.ObterAsync(dto.Id);

        if (propriedade is null)
            return Result.NotFound("Propriedade não localizada.");
        if (propriedade.ProprietarioId != dto.UsuarioId)
            return Result.Forbidden("Usuário não é o proprietário da propriedade.");

        _unitOfWork.PropriedadeRepository.Remover(propriedade);
        await _unitOfWork.CommitAsync(cancellationToken);

        return propriedade.ToDTO();
    }
}
