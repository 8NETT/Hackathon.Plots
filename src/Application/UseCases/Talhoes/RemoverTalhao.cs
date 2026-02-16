using Application.DTOs;
using Application.Mapping;
using Application.Persistence;

namespace Application.UseCases.Talhoes;

public sealed class RemoverTalhao : BaseUseCase<RemoverTalhaoDTO, TalhaoDTO>, IRemoverTalhaoUseCase
{
    public RemoverTalhao(IUnitOfWork unitOfWork, IValidator<RemoverTalhaoDTO>? validator, ILogger logger)
        : base(unitOfWork, validator, logger) { }

    protected override async Task<Result<TalhaoDTO>> ExecuteCoreAsync(RemoverTalhaoDTO dto, CancellationToken cancellationToken = default)
    {
        var talhao = await _unitOfWork.TalhaoRepository.ObterComPropriedade(dto.Id);

        if (talhao is null)
            return Result.NotFound("Talhão não encontrado.");
        if (talhao.Propriedade.ProprietarioId != dto.UsuarioId)
            return Result.Forbidden("Usuário não é o proprietário do talhão.");

        _unitOfWork.TalhaoRepository.Remover(talhao);
        await _unitOfWork.CommitAsync();

        return talhao.ToDTO();
    }
}
