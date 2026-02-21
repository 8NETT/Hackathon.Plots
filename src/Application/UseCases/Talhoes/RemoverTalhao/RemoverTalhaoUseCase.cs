using Application.DTOs;
using Application.Mapping;
using Application.Messaging;
using Application.Messaging.Events;
using Application.Persistence;

namespace Application.UseCases.Talhoes.RemoverTalhao;

public sealed class RemoverTalhaoUseCase : BaseUseCase<RemoverTalhaoDTO, TalhaoDTO>, IRemoverTalhaoUseCase
{
    private readonly IEventPublisher _eventPublisher;

    public RemoverTalhaoUseCase(IEventPublisher eventPublisher, IUnitOfWork unitOfWork, ILoggerFactory loggerFactory)
        : base(unitOfWork, loggerFactory)
    {
        _eventPublisher = eventPublisher; 
    }

    protected override async Task<Result<TalhaoDTO>> ExecuteCoreAsync(RemoverTalhaoDTO dto, CancellationToken cancellation = default)
    {
        var talhao = await _unitOfWork.TalhaoRepository.ObterComPropriedade(dto.Id, cancellation);

        if (talhao is null)
            return Result.NotFound("Talhão não encontrado.");
        if (talhao.Propriedade.ProprietarioId != dto.UsuarioId)
            return Result.Forbidden("Usuário não é o proprietário do talhão.");

        _unitOfWork.TalhaoRepository.Remover(talhao);
        await _unitOfWork.CommitAsync(cancellation);

        var @event = new TalhaoRemovidoEvent(talhao.Id);
        await _eventPublisher.PublishAsync(@event, cancellation);

        return talhao.ToDTO();
    }
}
