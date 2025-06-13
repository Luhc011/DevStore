using MediatR;

namespace DevStore.Vendas.Application.Events;

public class PedidoEventHandler :
    INotificationHandler<PedidoRascunhoIniciadoEvent>,
    INotificationHandler<PedidoItemAdicionadoEvent>
{
    public Task Handle(PedidoRascunhoIniciadoEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Handle(PedidoItemAdicionadoEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}