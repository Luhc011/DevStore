using DevStore.Core.Communication.Mediator;
using DevStore.Core.Messages;
using DevStore.Core.Messages.CommonMessages.Notifications;
using DevStore.Vendas.Application.Events;
using DevStore.Vendas.Domain;
using MediatR;

namespace DevStore.Vendas.Application.Commands;

public class PedidoCommandHandler :
    IRequestHandler<AdicionarItemPedidoCommand, bool>,
    IRequestHandler<AtualizarItemPedidoCommand, bool>,
    IRequestHandler<RemoverItemPedidoCommand, bool>,
    IRequestHandler<AplicarVoucherPedidoCommand, bool>,
    IRequestHandler<IniciarPedidoCommand, bool>
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IMediatorHandler _mediatorHandler;

    public PedidoCommandHandler(IPedidoRepository pedidoRepository, IMediatorHandler mediatorHandler)
    {
        _pedidoRepository = pedidoRepository;
        _mediatorHandler = mediatorHandler;
    }

    public async Task<bool> Handle(AdicionarItemPedidoCommand message, CancellationToken cancellationToken)
    {
        if (!ValidarComando(message)) return false;

        var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(message.ClienteId);
        var pedidoItem = new PedidoItem(message.ProdutoId, message.Nome, message.Quantidade, message.ValorUnitario);

        if (pedido is null)
        {
            pedido = Pedido.PedidoFactory.NovoPedidoRascunho(message.ClienteId);
            pedido.AdicionarItem(pedidoItem);

            _pedidoRepository.Adicionar(pedido);
            pedido.AdicionarEvento(new PedidoRascunhoIniciadoEvent(message.ClienteId, pedido.Id));
        }
        else
        {
            var pedidoItemExistente = pedido.PedidoItemExistente(pedidoItem);
            pedido.AdicionarItem(pedidoItem);

            if (pedidoItemExistente)
            {
                _pedidoRepository.AtualizarItem(pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId));
            }
            else
            {
                _pedidoRepository.AdicionarItem(pedidoItem);
            }
        }

        pedido.AdicionarEvento(new PedidoItemAdicionadoEvent(pedido.ClienteId, pedido.Id, message.ProdutoId, message.Nome, message.ValorUnitario, message.Quantidade));
        return await _pedidoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(AtualizarItemPedidoCommand message, CancellationToken cancellationToken)
    {
        if (!ValidarComando(message)) return false;

        var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(message.ClienteId);

        if (pedido is null)
        {
            await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Pedido não encontrado"));
            return false;
        }

        var pedidoItem = await _pedidoRepository.ObterItemPorPedido(pedido.Id, message.ProdutoId);

        if (!pedido.PedidoItemExistente(pedidoItem))
        {
            await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Item do pedido não encontrado"));
            return false;
        }

        pedido.AtualizarUnidades(pedidoItem, message.Quantidade);
        pedido.AdicionarEvento(new PedidoProdutoAtualizadoEvent(message.ClienteId, pedido.Id, message.ProdutoId, message.Quantidade));

        _pedidoRepository.AtualizarItem(pedidoItem);
        _pedidoRepository.Atualizar(pedido);

        return await _pedidoRepository.UnitOfWork.Commit();
    }
    public async Task<bool> Handle(RemoverItemPedidoCommand message, CancellationToken cancellationToken)
    {
        if (!ValidarComando(message)) return false;

        var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(message.ClienteId);

        if (pedido is null)
        {
            await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Pedido não encontrado!"));
            return false;
        }

        var pedidoItem = await _pedidoRepository.ObterItemPorPedido(pedido.Id, message.ProdutoId);

        if (pedidoItem != null && !pedido.PedidoItemExistente(pedidoItem))
        {
            await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Item do pedido não encontrado!"));
            return false;
        }

        pedido.RemoverItem(pedidoItem);
        pedido.AdicionarEvento(new PedidoProdutoRemovidoEvent(message.ClienteId, pedido.Id, message.ProdutoId));

        _pedidoRepository.RemoverItem(pedidoItem);
        _pedidoRepository.Atualizar(pedido);

        return await _pedidoRepository.UnitOfWork.Commit();
    }
    public async Task<bool> Handle(AplicarVoucherPedidoCommand message, CancellationToken cancellationToken)
    {
        if (!ValidarComando(message)) return false;

        var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(message.ClienteId);

        if (pedido is null)
        {
            await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Pedido não encontrado!"));
            return false;
        }

        var voucher = await _pedidoRepository.ObterVoucherPorCodigo(message.CodigoVoucher);

        if (voucher is null)
        {
            await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Voucher não encontrado!"));
            return false;
        }

        var voucherAplicacaoValidation = pedido.AplicarVoucher(voucher);
        if (!voucherAplicacaoValidation.IsValid)
        {
            foreach (var error in voucherAplicacaoValidation.Errors)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification(error.ErrorCode, error.ErrorMessage));
            }

            return false;
        }

        pedido.AdicionarEvento(new VoucherAplicadoPedidoEvent(message.ClienteId, pedido.Id, voucher.Id));

        _pedidoRepository.Atualizar(pedido);

        return await _pedidoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(IniciarPedidoCommand message, CancellationToken cancellationToken)
    {
        if (!ValidarComando(message)) return false;

        var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(message.ClienteId);
        pedido.IniciarPedido();
    }

    private bool ValidarComando(Command message)
    {
        if (message.EhValido()) return true;

        foreach (var error in message.ValidationResult.Errors)
        {
            _mediatorHandler.PublicarNotificacao(new DomainNotification(message.MessageType, error.ErrorMessage));
        }

        return false;
    }
}
