using Application.DTOs;
using Application.Mapping;
using Application.Persistence;

namespace Application.UseCases.Talhoes;

public sealed class ObterTalhao : BaseUseCase<ObterTalhaoDTO, TalhaoDTO>, IObterTalhaoUseCase
{
    public ObterTalhao(IUnitOfWork unitOfWork, IValidator<ObterTalhaoDTO>? validator, ILogger logger)
        : base(unitOfWork, validator, logger) { }

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
