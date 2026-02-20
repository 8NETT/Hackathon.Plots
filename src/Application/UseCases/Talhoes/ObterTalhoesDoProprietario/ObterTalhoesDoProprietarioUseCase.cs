using Application.DTOs;
using Application.Mapping;
using Application.Persistence;

namespace Application.UseCases.Talhoes.ObterTalhoesDoProprietario;

public sealed class ObterTalhoesDoProprietarioUseCase : BaseUseCase<Guid, IEnumerable<TalhaoDTO>>, IObterTalhoesDoProprietarioUseCase
{
    public ObterTalhoesDoProprietarioUseCase(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory)
        : base(unitOfWork, loggerFactory) { }

    protected override async Task<Result<IEnumerable<TalhaoDTO>>> ExecuteCoreAsync(Guid usuarioId, CancellationToken cancellationToken = default)
    {
        var talhoes = await _unitOfWork.TalhaoRepository.ObterDoProprietario(usuarioId);
        
        return Result.Success(talhoes.Select(t => t.ToDTO()));
    }
}
