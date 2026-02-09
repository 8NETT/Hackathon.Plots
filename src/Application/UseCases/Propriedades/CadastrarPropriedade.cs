using Application.DTOs;
using Application.Mapping;
using Application.Persistence;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;

namespace Application.UseCases.Propriedades;

public sealed class CadastrarPropriedade : IUseCase<CadastrarPropriedadeDTO, Result<PropriedadeDTO>>
{
    private IValidator<CadastrarPropriedadeDTO>? _validator;
    private IUnitOfWork _unitOfWork;

    public CadastrarPropriedade(IUnitOfWork unitOfWork, IValidator<CadastrarPropriedadeDTO>? validator)
    {
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PropriedadeDTO>> HandleAsync(CadastrarPropriedadeDTO dto)
    {
        var result = _validator?.Validate(dto);
        if (result is not null && !result.IsValid)
            return Result.Invalid(result.AsErrors());

        var propriedade = dto.ToEntity();

        _unitOfWork.PropriedadeRepository.Cadastrar(propriedade);
        await _unitOfWork.CommitAsync();

        return propriedade.ToDTO();
    }
}
