﻿using DevStore.Core.DomainObjects.DTO;

namespace DevStore.Core.Messages.CommonMessages.IntegrationEvents;

public class PedidoProcessamentoCanceladoEvent : IntegrationEvent
{
    public Guid PedidoId { get; private set; }
    public Guid ClienteId { get; private set; }
    public ListaProdutosPedido ProdutosPedido { get; private set; }

    public PedidoProcessamentoCanceladoEvent(Guid pedidoId, Guid clienteId, ListaProdutosPedido produtosPedido)
    {
        AggregateId = pedidoId;
        PedidoId = pedidoId;
        ClienteId = clienteId;
        ProdutosPedido = produtosPedido;
    }
}