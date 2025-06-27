using DevStore.Core.Messages;

namespace DevStore.Vendas.Application.Events;

public class PedidoFinalizadoEvent : Event
{
    public Guid PedidoId { get; private set; }

    public PedidoFinalizadoEvent(Guid pedidoId)
    {
        PedidoId = pedidoId;
        AggregateId = pedidoId;
    }
}