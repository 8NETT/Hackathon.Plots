using Application.DTOs;
using Application.Mapping;
using Application.Persistence;

namespace Application.UseCases.Talhoes.CadastrarTalhao;

public sealed class CadastrarTalhaoUseCase : BaseUseCase<CadastrarTalhaoDTO, TalhaoDTO>, ICadastrarTalhaoUseCase
{
    public CadastrarTalhaoUseCase(IUnitOfWork unitOfWork, IValidator<CadastrarTalhaoDTO>? validator, ILogger logger)
        : base(unitOfWork, validator, logger) { }

    protected override async Task<Result<TalhaoDTO>> ExecuteCoreAsync(CadastrarTalhaoDTO dto, CancellationToken cancellation = default)
    {
        var propriedade = await _unitOfWork.PropriedadeRepository.ObterAsync(dto.PropriedadeId, cancellation);

        if (propriedade is null)
            return Result.NotFound("Propriedade não localizada.");
        if (propriedade.ProprietarioId != dto.UsuarioId)
            return Result.Forbidden("Usuário não é proprietário da propriedade.");

        var talhao = dto.ToEntity();

        _unitOfWork.TalhaoRepository.Cadastrar(talhao);
        await _unitOfWork.CommitAsync();

        return talhao.ToDTO();
    }
}
