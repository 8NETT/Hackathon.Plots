using Application.DTOs;
using Application.Mapping;
using Application.Persistence;

namespace Application.UseCases.Propriedades.AlterarPropriedade;

public sealed class AlterarPropriedadeUseCase : BaseUseCase<AlterarPropriedadeDTO, PropriedadeDTO>, IAlterarPropriedadeUseCase
{
    public AlterarPropriedadeUseCase(IUnitOfWork unitOfWork, IValidator<AlterarPropriedadeDTO>? validator, ILogger logger)
        : base(unitOfWork, validator, logger) { }

    protected override async Task<Result<PropriedadeDTO>> ExecuteCoreAsync(AlterarPropriedadeDTO dto, CancellationToken cancellationToken = default)
    {
        var propriedade = await _unitOfWork.PropriedadeRepository.ObterAsync(dto.Id);

        if (propriedade is null)
            return Result.NotFound("Propriedade não localizada.");
        if (propriedade.ProprietarioId != dto.UsuarioId)
            return Result.Forbidden("Usuário não é o proprietário.");

        propriedade.AlterarIdentificacao(dto.Nome, dto.Descricao);
        propriedade.AlterarEndereco(dto.Endereco?.ToValueObject());

        _unitOfWork.PropriedadeRepository.Atualizar(propriedade);
        await _unitOfWork.CommitAsync(cancellationToken);

        return propriedade.ToDTO();
    }
}
