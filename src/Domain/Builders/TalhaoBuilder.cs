using Domain.Entities;
using Domain.Exceptions;
using Domain.Extensions;
using Domain.Validators;
using Domain.ValueObjects;
using FluentValidation.Results;

namespace Domain.Builders;

public sealed class TalhaoBuilder
{
    private Talhao _talhao = new();

    public TalhaoBuilder PropriedadeId(Guid id) => this.Tee(b => b._talhao.PropriedadeId = id);
    public TalhaoBuilder Nome(string nome) => this.Tee(b => b._talhao.Nome = nome);
    public TalhaoBuilder Descricao(string? descricao) => this.Tee(b => b._talhao.Descricao = descricao);
    public TalhaoBuilder Coordenadas(Coordenadas coordenadas) => this.Tee(b => b._talhao.Coordenadas = coordenadas);
    public TalhaoBuilder Coordenadas(double latitude, double longitude) => this.Tee(b => b._talhao.Coordenadas = new Coordenadas(latitude, longitude));
    public TalhaoBuilder Area(decimal area) => this.Tee(b => b._talhao.Area = area);

    public ValidationResult Validate() =>
        new TalhaoValidator().Validate(_talhao);

    public Talhao Build()
    {
        if (!Validate().IsValid)
            throw new EstadoInvalidoException("Não é possível criar um talhão num estado inválido.");

        return _talhao;
    }
}
