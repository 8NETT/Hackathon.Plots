using Application.DTOs;
using Application.Mapping;
using Application.Persistence;
using Ardalis.Result;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Propriedades;

public sealed class CadastrarPropriedade : BaseUseCase<CadastrarPropriedadeDTO, PropriedadeDTO>, ICadastrarPropriedadeUseCase
{
    public CadastrarPropriedade(
        IUnitOfWork unitOfWork, 
        IValidator<CadastrarPropriedadeDTO>? validator,
        ILogger<CadastrarPropriedade> logger) 
        : base(unitOfWork, validator, logger) { }

    protected override async Task<Result<PropriedadeDTO>> ExecuteCoreAsync(CadastrarPropriedadeDTO input, CancellationToken cancellationToken = default)
    {
        var propriedade = input.ToEntity();

        _unitOfWork.PropriedadeRepository.Cadastrar(propriedade);
        await _unitOfWork.CommitAsync(cancellationToken);

        return propriedade.ToDTO();
    }
}
