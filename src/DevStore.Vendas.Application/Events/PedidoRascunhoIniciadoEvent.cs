﻿using DevStore.Core.Messages;

namespace DevStore.Vendas.Application.Events;

public class PedidoRascunhoIniciadoEvent : Event
{
    public Guid ClienteId { get; private set; }
    public Guid PedidoId { get; private set; }

    public PedidoRascunhoIniciadoEvent(Guid clienteId, Guid pedidoId)
    {
        AggregateId = pedidoId;
        ClienteId = clienteId;
        PedidoId = pedidoId;
    }
}
