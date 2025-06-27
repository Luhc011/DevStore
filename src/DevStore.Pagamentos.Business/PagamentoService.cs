using DevStore.Core.Communication.Mediator;
using DevStore.Core.DomainObjects.DTO;
using DevStore.Core.Messages.CommonMessages.IntegrationEvents;
using DevStore.Core.Messages.CommonMessages.Notifications;
using DevStore.Pagamentos.Business.Interfaces;

namespace DevStore.Pagamentos.Business;

public class PagamentoService : IPagamentoService
{
    private readonly IPagamentoCartaoCreditoFacade _pagamentoCartaoCreditoFacade;
    private readonly IPagamentoRepository _repository;
    private readonly IMediatorHandler _mediator;

    public PagamentoService(IPagamentoCartaoCreditoFacade pagamentoCartaoCreditoFacade,
                            IPagamentoRepository repository,
                            IMediatorHandler mediator)
    {
        _pagamentoCartaoCreditoFacade = pagamentoCartaoCreditoFacade;
        _repository = repository;
        _mediator = mediator;
    }

    public async Task<Transacao> RealizarPagamentoPedido(PagamentoPedido pagamentoPedido)
    {
        var pedido = new Pedido
        {
            Id = pagamentoPedido.PedidoId,
            Valor = pagamentoPedido.Total
        };

        var pagamento = new Pagamento
        {
            Valor = pagamentoPedido.Total,
            NomeCartao = pagamentoPedido.NomeCartao,
            NumeroCartao = pagamentoPedido.NumeroCartao,
            ExpiracaoCartao = pagamentoPedido.ExpiracaoCartao,
            CvvCartao = pagamentoPedido.CvvCartao,
            PedidoId = pagamentoPedido.PedidoId
        };

        var transacao = _pagamentoCartaoCreditoFacade.RealizarPagamento(pedido, pagamento);

        if (transacao.StatusTransacao == StatusTransacao.Pago)
        {
            pagamento.AdicionarEvento(new PedidoPagamentoRealizadoEvent(pedido.Id, pagamentoPedido.ClienteId, transacao.PagamentoId, transacao.Id, pedido.Valor));

            _repository.Adicionar(pagamento);
            _repository.AdicionarTransacao(transacao);

            await _repository.UnitOfWork.Commit();
            return transacao;
        }

        await _mediator.PublicarNotificacao(new DomainNotification("pagamento", "A operadora recusou o pagamento"));
        await _mediator.PublicarEvento(new PedidoPagamentoRecusadoEvent(pedido.Id, pagamentoPedido.ClienteId,
            transacao.PagamentoId, transacao.Id, pedido.Valor));

        return transacao;
    }
}