using Domain.Entities;
using Domain.Exceptions;
using Domain.Extensions;
using Domain.Validators;
using Domain.ValueObjects;
using FluentValidation.Results;

namespace Domain.Builders;

public sealed class PropriedadeBuilder
{
    private Propriedade _propriedade = new();

    public PropriedadeBuilder ProprietarioId(Guid id) => this.Tee(b => b._propriedade.ProprietarioId = id);
    public PropriedadeBuilder Nome(string nome) => this.Tee(b => b._propriedade.Nome = nome);
    public PropriedadeBuilder Descricao(string? descricao) => this.Tee(b => b._propriedade.Descricao = descricao);
    public PropriedadeBuilder Endereco(Endereco? endereco) => this.Tee(b => b._propriedade.Endereco = endereco);

    public ValidationResult Validate() =>
        new PropriedadeValidator().Validate(_propriedade);

    public Propriedade Build()
    {
        if (!Validate().IsValid)
            throw new EstadoInvalidoException("Não é possível criar uma propriedade em um estado inválido.");

        return _propriedade;
    }
}
