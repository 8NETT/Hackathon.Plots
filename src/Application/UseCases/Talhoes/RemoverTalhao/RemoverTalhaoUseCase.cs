using Application.DTOs;
using Application.Mapping;
using Application.Persistence;

namespace Application.UseCases.Talhoes.RemoverTalhao;

public sealed class RemoverTalhaoUseCase : BaseUseCase<RemoverTalhaoDTO, TalhaoDTO>, IRemoverTalhaoUseCase
{
    public RemoverTalhaoUseCase(IUnitOfWork unitOfWork, ILogger logger)
        : base(unitOfWork, logger) { }

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
