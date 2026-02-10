using Application.DTOs;
using Application.Mapping;
using Application.Persistence;
using Ardalis.Result;
using FluentValidation;

namespace Application.UseCases.Propriedades;

public sealed class CadastrarPropriedade : ResultUseCase<CadastrarPropriedadeDTO, PropriedadeDTO>
{
    public CadastrarPropriedade(IUnitOfWork unitOfWork, IValidator<CadastrarPropriedadeDTO>? validator) : base(unitOfWork, validator) { }

    protected override async Task<Result<PropriedadeDTO>> ExecuteCoreAsync(CadastrarPropriedadeDTO input, CancellationToken cancellationToken = default)
    {
        var propriedade = input.ToEntity();

        _unitOfWork.PropriedadeRepository.Cadastrar(propriedade);
        await _unitOfWork.CommitAsync(cancellationToken);

        return propriedade.ToDTO();
    }
}
