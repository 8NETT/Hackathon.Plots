using Application.Validators;

namespace Application.UseCases.Talhoes.CadastrarTalhao;

internal sealed class CadastrarTalhaoValidator
    : AbstractValidator<CadastrarTalhaoDTO>
{
    public CadastrarTalhaoValidator()
    {
        RuleFor(d => d.UsuarioId)
            .NotEmpty().WithMessage("O UsuarioId é obrigatório.");
        RuleFor(d => d.PropriedadeId)
            .NotEmpty().WithMessage("A PropriedadeId é obrigatória.");
        RuleFor(d => d.Nome)
            .NotEmpty().WithMessage("O Nome é obrigatório.")
            .Length(1, 100).WithMessage("O Nome deve ter entre 1 a 100 caracteres.");
        RuleFor(d => d.Descricao)
            .MaximumLength(500).WithMessage("A Descricao deve ter no máximo 500 caracteres.");
        RuleFor(d => d.Coordenadas)
            .SetValidator(d => new CoordenadasValidator());
        RuleFor(d => d.Area)
            .NotEmpty().WithMessage("A Area é obrigatória.")
            .GreaterThan(0).WithMessage("A Area deve ser um valor positivo.");
    }
}
