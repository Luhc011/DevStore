using DevStore.Core.Messages.CommonMessages.IntegrationEvents;
using MediatR;

namespace DevStore.Vendas.Application.Events;

public class PedidoEventHandler :
    INotificationHandler<PedidoRascunhoIniciadoEvent>,
    INotificationHandler<PedidoItemAdicionadoEvent>,
    INotificationHandler<PedidoEstoqueRejeitadoEvent>
{
    public Task Handle(PedidoRascunhoIniciadoEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Handle(PedidoItemAdicionadoEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Handle(PedidoEstoqueRejeitadoEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}