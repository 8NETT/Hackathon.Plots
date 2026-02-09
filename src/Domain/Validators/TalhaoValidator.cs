using Domain.Entities;
using FluentValidation;

namespace Domain.Validators;

internal sealed class TalhaoValidator : AbstractValidator<Talhao>
{
    public TalhaoValidator()
    {
        RuleFor(t => t.PropriedadeId)
            .NotEmpty().WithMessage("A propriedade é obrigatória.");
        RuleFor(t => t.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.");
        RuleFor(t => t.Coordenadas)
            .NotNull().WithMessage("A coordenada é obrigatória.");
        RuleFor(t => t.Area)
            .GreaterThanOrEqualTo(0M).WithMessage("A área não pode ser negativa.");
    }
}
