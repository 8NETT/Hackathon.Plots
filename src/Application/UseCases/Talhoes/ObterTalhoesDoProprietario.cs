using Application.DTOs;
using Application.Mapping;
using Application.Persistence;

namespace Application.UseCases.Talhoes;

public sealed class ObterTalhoesDoProprietario : BaseUseCase<Guid, IEnumerable<TalhaoDTO>>, IObterTalhoesDoProprietario
{
    public ObterTalhoesDoProprietario(IUnitOfWork unitOfWork, ILogger logger)
        : base(unitOfWork, logger) { }

    protected override async Task<Result<IEnumerable<TalhaoDTO>>> ExecuteCoreAsync(Guid usuarioId, CancellationToken cancellationToken = default)
    {
        var talhoes = await _unitOfWork.TalhaoRepository.ObterDoProprietario(usuarioId);
        
        return Result.Success(talhoes.Select(t => t.ToDTO()));
    }
}
