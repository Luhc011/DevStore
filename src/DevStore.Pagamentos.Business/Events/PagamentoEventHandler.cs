﻿using DevStore.Core.DomainObjects.DTO;
using DevStore.Core.Messages;
using DevStore.Core.Messages.CommonMessages.IntegrationEvents;
using DevStore.Pagamentos.Business.Interfaces;
using MediatR;

namespace DevStore.Pagamentos.Business.Events;

public class PagamentoEventHandler : INotificationHandler<PedidoEstoqueConfirmadoEvent>
{
    private readonly IPagamentoService _pagamentoService;

    public PagamentoEventHandler(IPagamentoService pagamentoService)
    {
        _pagamentoService = pagamentoService;
    }

    public async Task Handle(PedidoEstoqueConfirmadoEvent message, CancellationToken cancellationToken)
    {
        var pagamentoPedido = new PagamentoPedido
        {
            PedidoId = message.PedidoId,
            ClienteId = message.ClienteId,
            Total = message.Total,
            NomeCartao = message.NomeCartao,
            NumeroCartao = message.NumeroCartao,
            ExpiracaoCartao = message.ExpiracaoCartao,
            CvvCartao = message.CvvCartao
        };

        await _pagamentoService.RealizarPagamentoPedido(pagamentoPedido);
    }
}
