using Application.DTOs;
using Application.Mapping;
using Application.Messaging;
using Application.Messaging.Events;
using Application.Persistence;

namespace Application.UseCases.Talhoes.CadastrarTalhao;

public sealed class CadastrarTalhaoUseCase : BaseUseCase<CadastrarTalhaoDTO, TalhaoDTO>, ICadastrarTalhaoUseCase
{
    private readonly IEventPublisher _eventPublisher;

    public CadastrarTalhaoUseCase(
        IEventPublisher eventPublisher,
        IUnitOfWork unitOfWork, 
        IValidator<CadastrarTalhaoDTO>? validator, 
        ILoggerFactory loggerFactory)
        : base(unitOfWork, validator, loggerFactory)
    {
        _eventPublisher = eventPublisher;
    }

    protected override async Task<Result<TalhaoDTO>> ExecuteCoreAsync(CadastrarTalhaoDTO dto, CancellationToken cancellation = default)
    {
        var propriedade = await _unitOfWork.PropriedadeRepository.ObterAsync(dto.PropriedadeId, cancellation);

        if (propriedade is null)
            return Result.NotFound("Propriedade não localizada.");
        if (propriedade.ProprietarioId != dto.UsuarioId)
            return Result.Forbidden("Usuário não é proprietário da propriedade.");

        var talhao = dto.ToEntity();

        _unitOfWork.TalhaoRepository.Cadastrar(talhao);
        await _unitOfWork.CommitAsync(cancellation);

        var @event = new TalhaoCriadoEvent(talhao, propriedade);
        await _eventPublisher.PublishAsync(@event, cancellation);

        return talhao.ToDTO();
    }
}
