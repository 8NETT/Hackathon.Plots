using Application.DTOs;
using Application.Mapping;
using Application.Persistence;

namespace Application.UseCases.Propriedades.ObterPropriedadesDoProprietario;

public sealed class ObterPropriedadesDoProprietarioUseCase
    : BaseUseCase<Guid, IEnumerable<PropriedadeDTO>>, IObterPropriedadesDoProprietarioUseCase
{
    public ObterPropriedadesDoProprietarioUseCase(IUnitOfWork unitOfWork, ILogger logger)
        : base(unitOfWork, logger) { }

    protected override async Task<Result<IEnumerable<PropriedadeDTO>>> ExecuteCoreAsync(Guid usuarioId, CancellationToken cancellation = default)
    {
        var propriedades = await _unitOfWork.PropriedadeRepository.ObterDoProprietarioAsync(usuarioId, cancellation);
        var dtos = propriedades.Select(p => p.ToDTO());

        return Result.Success(dtos);
    }
}
