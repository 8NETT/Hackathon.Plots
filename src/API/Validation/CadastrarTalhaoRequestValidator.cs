using API.Requests;
using Application.Extensions;

namespace API.Validation;

internal sealed class CadastrarTalhaoRequestValidator
    : AbstractValidator<CadastrarTalhaoRequest>
{
    public CadastrarTalhaoRequestValidator()
    {
        RuleFor(d => d.Nome)
            .NotEmpty().WithMessage("O Nome é obrigatório.")
            .Length(1, 100).WithMessage("O Nome deve ter entre 1 a 100 caracteres.");
        RuleFor(d => d.Descricao)
            .MaximumLength(500).WithMessage("A Descricao deve ter no máximo 500 caracteres.");
        RuleFor(d => d.Coordenadas)
            .NotEmpty().WithMessage("As Coordenadas são obrigatórias.")
            .SetOptionalValidator(new CoordenadasRequestValidator());
        RuleFor(d => d.Area)
            .NotEmpty().WithMessage("A Area é obrigatória.")
            .GreaterThan(0).WithMessage("A Area deve ser um valor positivo.");
    }
}
