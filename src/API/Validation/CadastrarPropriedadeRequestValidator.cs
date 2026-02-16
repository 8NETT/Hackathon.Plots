using API.Requests;
using Application.Extensions;

namespace API.Validation;

public class CadastrarPropriedadeRequestValidator : AbstractValidator<CadastrarPropriedadeRequest>
{
    public CadastrarPropriedadeRequestValidator()
    {
        RuleFor(d => d.Nome)
            .NotEmpty().WithMessage("O Nome é obrigatório.")
            .Length(1, 100).WithMessage("O Nome deve ter entre 1 a 100 caracteres.");
        RuleFor(d => d.Descricao)
            .MaximumLength(500).WithMessage("A Descrição deve ter no máximo 500 caracteres.");
        RuleFor(d => d.Endereco)
            .SetOptionalValidator(new EnderecoRequestValidator());
    }
}
