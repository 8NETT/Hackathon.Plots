using Application.DTOs;

namespace Application.Validators;

internal sealed class CoordenadasValidator
    : AbstractValidator<CoordenadasDTO>
{
    public CoordenadasValidator()
    {
        RuleFor(d => d.Latitude)
            .NotEmpty().WithMessage("A Latitude é obrigatória.")
            .GreaterThanOrEqualTo(-90).WithMessage("A Latitude deve ser maior ou igual a -90º.")
            .LessThanOrEqualTo(90).WithMessage("A Latitude deve ser menor ou igual a 90º.");
        RuleFor(d => d.Longitude)
            .NotEmpty().WithMessage("A Longitude deve ser preenchida.")
            .GreaterThanOrEqualTo(-180).WithMessage("A Longitude deve ser maior ou igual a -180º.")
            .LessThanOrEqualTo(180).WithMessage("A Longitude deve ser menor ou igual a 180º.");
    }
}
