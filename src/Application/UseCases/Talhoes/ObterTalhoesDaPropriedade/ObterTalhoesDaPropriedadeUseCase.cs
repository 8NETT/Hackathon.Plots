using Application.DTOs;
using Application.Mapping;
using Application.Persistence;

namespace Application.UseCases.Talhoes.ObterTalhoesDaPropriedade;

public sealed class ObterTalhoesDaPropriedadeUseCase : BaseUseCase<ObterTalhoesDaPropriedadeDTO, IEnumerable<TalhaoDTO>>, IObterTalhoesDaPropriedadeUseCase
{
    public ObterTalhoesDaPropriedadeUseCase(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory)
        : base(unitOfWork, loggerFactory) { }

    protected override async Task<Result<IEnumerable<TalhaoDTO>>> ExecuteCoreAsync(ObterTalhoesDaPropriedadeDTO dto, CancellationToken cancellationToken = default)
    {
        var propriedade = await _unitOfWork.PropriedadeRepository.ObterComTalhoesAsync(dto.PropriedadeId);

        if (propriedade is null)
            return Result.NotFound("Propriedade não localizada.");
        if (propriedade.ProprietarioId != dto.UsuarioId)
            return Result.Forbidden("Usuário não é proprietário dos talhões.");

        return Result.Success(propriedade.Talhoes.Select(t => t.ToDTO()));
    }
}
