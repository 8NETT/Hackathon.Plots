using Application.DTOs;
using Application.Mapping;
using Application.Persistence;

namespace Application.UseCases.Talhoes.AlterarTalhao;

public sealed class AlterarTalhaoUseCase : BaseUseCase<AlterarTalhaoDTO, TalhaoDTO>, IAlterarTalhaoUseCase
{
    public AlterarTalhaoUseCase(IUnitOfWork unitOfWork, IValidator<AlterarTalhaoDTO>? validator, ILoggerFactory loggerFactory)
        : base(unitOfWork, validator, loggerFactory) { }

    protected override async Task<Result<TalhaoDTO>> ExecuteCoreAsync(AlterarTalhaoDTO dto, CancellationToken cancellationToken = default)
    {
        var talhao = await _unitOfWork.TalhaoRepository.ObterComPropriedade(dto.Id);

        if (talhao is null)
            return Result.NotFound("Talhão não localizado.");
        if (talhao.Propriedade.ProprietarioId != dto.UsuarioId)
            return Result.Forbidden("Usuário não é proprietário do talhão.");

        talhao.Alterar(dto.Nome, dto.Descricao, dto.Coordenadas.ToValueObject(), dto.Area);

        _unitOfWork.TalhaoRepository.Atualizar(talhao);
        await _unitOfWork.CommitAsync();

        return talhao.ToDTO();
    }
}
