using Application.DTOs;
using Application.Mapping;
using Application.Messaging;
using Application.Messaging.Events;
using Application.Persistence;

namespace Application.UseCases.Talhoes.AlterarTalhao;

public sealed class AlterarTalhaoUseCase : BaseUseCase<AlterarTalhaoDTO, TalhaoDTO>, IAlterarTalhaoUseCase
{
    private readonly IEventPublisher _eventPublisher;

    public AlterarTalhaoUseCase(
        IEventPublisher eventPublisher,
        IUnitOfWork unitOfWork, 
        IValidator<AlterarTalhaoDTO>? validator, 
        ILoggerFactory loggerFactory)
        : base(unitOfWork, validator, loggerFactory)
    {
        _eventPublisher = eventPublisher;
    }

    protected override async Task<Result<TalhaoDTO>> ExecuteCoreAsync(AlterarTalhaoDTO dto, CancellationToken cancellation = default)
    {
        var talhao = await _unitOfWork.TalhaoRepository.ObterComPropriedade(dto.Id, cancellation);

        if (talhao is null)
            return Result.NotFound("Talhão não localizado.");
        if (talhao.Propriedade.ProprietarioId != dto.UsuarioId)
            return Result.Forbidden("Usuário não é proprietário do talhão.");

        talhao.Alterar(dto.Nome, dto.Descricao, dto.Coordenadas.ToValueObject(), dto.Area);

        _unitOfWork.TalhaoRepository.Atualizar(talhao);
        await _unitOfWork.CommitAsync(cancellation);

        var @event = new TalhaoAlteradoEvent(talhao, talhao.Propriedade);
        await _eventPublisher.PublishAsync(@event, cancellation);

        return talhao.ToDTO();
    }
}
