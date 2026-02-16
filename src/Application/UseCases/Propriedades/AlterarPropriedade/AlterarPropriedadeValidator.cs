using Application.Extensions;
using Application.Validators;

namespace Application.UseCases.Propriedades.AlterarPropriedade;

internal sealed class AlterarPropriedadeValidator :
    AbstractValidator<AlterarPropriedadeDTO>
{
    public AlterarPropriedadeValidator()
    {
        RuleFor(d => d.Id)
            .NotEmpty().WithMessage("O Id é obrigatório.");
        RuleFor(d => d.UsuarioId)
            .NotEmpty().WithMessage("O UsuarioId é obrigatório.");
        RuleFor(d => d.Nome)
            .NotEmpty().WithMessage("O Nome é obrigatório.")
            .Length(1, 100).WithMessage("O Nome deve ter entre 1 a 100 caracteres.");
        RuleFor(d => d.Descricao)
            .MaximumLength(500).WithMessage("A Descrição deve ter no máximo 500 caracteres.");
        RuleFor(d => d.Endereco)
            .SetOptionalValidator(new EnderecoValidator());
    }
}
