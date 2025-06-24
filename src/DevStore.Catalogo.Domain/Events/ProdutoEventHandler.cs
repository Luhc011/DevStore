using DevStore.Core.Communication.Mediator;
using DevStore.Core.Messages.CommonMessages.IntegrationEvents;
using MediatR;

namespace DevStore.Catalogo.Domain.Events;

public class ProdutoEventHandler :
    INotificationHandler<ProdutoAbaixoEstoqueEvent>,
    INotificationHandler<PedidoIniciadoEvent>
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IEstoqueService _estoqueService;
    private readonly IMediatorHandler _mediatorHandler;

    public ProdutoEventHandler(IProdutoRepository produtoRepository, IMediatorHandler mediatorHandler, IEstoqueService estoqueService)
    {
        _produtoRepository = produtoRepository;
        _mediatorHandler = mediatorHandler;
        _estoqueService = estoqueService;
    }

    public async Task Handle(ProdutoAbaixoEstoqueEvent notification, CancellationToken cancellationToken)
    {
        var produto = await _produtoRepository.ObterPorId(notification.AggregateId);

        // Enviar um email para aquisicao de mais produtos.
    }

    public async Task Handle(PedidoIniciadoEvent notification, CancellationToken cancellationToken)
    {
        var result = await _estoqueService.DebitarListaProdutosPedido(notification.ProdutosPedido);

        if (result)
        {
            await _mediatorHandler.PublicarEvento(new PedidoEstoqueConfirmadoEvent(notification.PedidoId, notification.ClienteId, notification.Total, notification.ProdutosPedido, notification.NomeCartao, notification.NumeroCartao, notification.ExpiracaoCartao, notification.CvvCartao));
        }
        else
        {
            await _mediatorHandler.PublicarEvento(new PedidoEstoqueRejeitadoEvent(notification.PedidoId, notification.ClienteId));
        }
    }
}