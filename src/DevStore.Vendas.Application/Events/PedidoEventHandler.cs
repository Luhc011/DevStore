﻿using DevStore.Core.Communication.Mediator;
using DevStore.Core.Messages;
using DevStore.Core.Messages.CommonMessages.IntegrationEvents;
using DevStore.Vendas.Application.Commands;
using MediatR;

namespace DevStore.Vendas.Application.Events;

public class PedidoEventHandler :
    INotificationHandler<PedidoRascunhoIniciadoEvent>,
    INotificationHandler<PedidoItemAdicionadoEvent>,
    INotificationHandler<PedidoEstoqueRejeitadoEvent>,
    INotificationHandler<PedidoPagamentoRealizadoEvent>,
    INotificationHandler<PedidoPagamentoRecusadoEvent>
{
    private readonly IMediatorHandler _mediatorHandler;

    public PedidoEventHandler(IMediatorHandler mediatorHandler)
    {
        _mediatorHandler = mediatorHandler;
    }

    public Task Handle(PedidoRascunhoIniciadoEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Handle(PedidoItemAdicionadoEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task Handle(PedidoEstoqueRejeitadoEvent message, CancellationToken cancellationToken)
    {
        await _mediatorHandler.EnviarComando(new CancelarProcessamentoPedidoCommand(message.PedidoId, message.ClienteId));
    }

    public async Task Handle(PedidoPagamentoRealizadoEvent message, CancellationToken cancellationToken)
    {
        await _mediatorHandler.EnviarComando(new FinalizarPedidoCommand(message.PedidoId, message.ClienteId));
    }

    public async Task Handle(PedidoPagamentoRecusadoEvent message, CancellationToken cancellationToken)
    {
        await _mediatorHandler.EnviarComando(new CancelarProcessamentoPedidoEstornarEstoqueCommand(message.PedidoId, message.ClienteId));
    }
}