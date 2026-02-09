using Domain.Entities;
using Domain.ValueObjects;
using FluentValidation;

namespace Domain.Validators;

internal sealed class PropriedadeValidator : AbstractValidator<Propriedade>
{
    public PropriedadeValidator()
    {
        RuleFor(p => p.ProprietarioId)
            .NotEmpty().WithMessage("O proprietário é obrigatório.");
        RuleFor(p => p.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.");
    }
}
