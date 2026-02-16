using Application.DTOs;

namespace Application.Validators;

internal sealed class EnderecoValidator : AbstractValidator<EnderecoDTO>
{
    public EnderecoValidator()
    {
        RuleFor(e => e.Logradouro)
            .NotEmpty().WithMessage("O Logradouro é obrigatório.")
            .Length(1, 200).WithMessage("O Logradouro deve ter entre 1 a 200 caracteres.");
        RuleFor(e => e.Numero)
            .NotEmpty().WithMessage("O Numero é obrigatório.")
            .Length(1, 15).WithMessage("O Numero deve ter entre 1 a 15 caracteres.");
        RuleFor(e => e.Complemento)
            .MaximumLength(100).WithMessage("O Complemento deve ter no máximo 100 caracteres.");
        RuleFor(e => e.Bairro)
            .MaximumLength(120).WithMessage("O Bairro deve ter no máximo 120 caracteres.");
        RuleFor(e => e.Cidade)
            .NotEmpty().WithMessage("A Cidade é obrigatória.")
            .Length(1, 120).WithMessage("A Cidade deve ter entre 1 a 120 caracteres.");
        RuleFor(e => e.UF)
            .NotEmpty().WithMessage("A UF é obrigatória.")
            .Length(2, 2).WithMessage("A UF deve ter 2 caracteres.");
        RuleFor(e => e.CEP)
            .NotEmpty().WithMessage("CEP é obrigatório.")
            .Matches(@"^\d{5}-?\d{3}$").WithMessage("CEP inválido. Use o formato 12345-678 ou 12345678.");
    }
}
