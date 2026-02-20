using Application.DTOs;
using Application.Mapping;
using Application.Persistence;

namespace Application.UseCases.Propriedades.CadastrarPropriedade;

public sealed class CadastrarPropriedadeUseCase : BaseUseCase<CadastrarPropriedadeDTO, PropriedadeDTO>, ICadastrarPropriedadeUseCase
{
    public CadastrarPropriedadeUseCase(
        IUnitOfWork unitOfWork, 
        IValidator<CadastrarPropriedadeDTO>? validator,
        ILoggerFactory loggerFactory) 
        : base(unitOfWork, validator, loggerFactory) { }

    protected override async Task<Result<PropriedadeDTO>> ExecuteCoreAsync(CadastrarPropriedadeDTO input, CancellationToken cancellationToken = default)
    {
        var propriedade = input.ToEntity();

        _unitOfWork.PropriedadeRepository.Cadastrar(propriedade);
        await _unitOfWork.CommitAsync(cancellationToken);

        return propriedade.ToDTO();
    }
}
