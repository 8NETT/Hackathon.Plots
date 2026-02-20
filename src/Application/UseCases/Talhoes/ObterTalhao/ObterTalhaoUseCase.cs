using Application.DTOs;
using Application.Mapping;
using Application.Persistence;

namespace Application.UseCases.Talhoes.ObterTalhao;

public sealed class ObterTalhaoUseCase : BaseUseCase<ObterTalhaoDTO, TalhaoDTO>, IObterTalhaoUseCase
{
    public ObterTalhaoUseCase(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory)
        : base(unitOfWork, loggerFactory) { }

    protected override async Task<Result<TalhaoDTO>> ExecuteCoreAsync(ObterTalhaoDTO dto, CancellationToken cancellationToken = default)
    {
        var talhao = await _unitOfWork.TalhaoRepository.ObterComPropriedade(dto.Id);

        if (talhao is null)
            return Result.NotFound("Talhão não localizado.");
        if (talhao.Propriedade.ProprietarioId != dto.UsuarioId)
            return Result.Forbidden("Usuário não é proprietário do talhão.");

        return talhao.ToDTO();
    }
}
