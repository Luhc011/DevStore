using DevStore.Vendas.Application.Queries.ViewModels;
using DevStore.Vendas.Domain;

namespace DevStore.Vendas.Application.Queries;

public class PedidoQueries : IPedidoQueries
{
    private readonly IPedidoRepository _pedidoRepository;

    public PedidoQueries(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public async Task<CarrinhoViewModel> ObterCarrinhoCliente(Guid clienteId)
    {
        var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(clienteId);
        if (pedido is null) return new();

        var carrinho = new CarrinhoViewModel
        {
            ClienteId = clienteId,
            ValorTotal = pedido.ValorTotal,
            PedidoId = pedido.Id,
            ValorDesconto = pedido.Desconto,
            SubTotal = pedido.Desconto + pedido.ValorTotal
        };

        if (pedido.VoucherId != null)
            carrinho.VoucherCodigo = pedido.Voucher.Codigo;

        foreach (var item in pedido.PedidoItems)
        {
            carrinho.Items.Add(new CarrinhoItemViewModel
            {
                ProdutoId = item.ProdutoId,
                ProdutoNome = item.ProdutoNome,
                Quantidade = item.Quantidade,
                ValorTotal = item.ValorUnitario * item.Quantidade,
                ValorUnitario = item.ValorUnitario
            });
        }

        return carrinho;
    }

    public async Task<IEnumerable<PedidoViewModel>> ObterPedidosCliente(Guid clienteId)
    {
        var pedidos = await _pedidoRepository.ObterListaPorClienteId(clienteId);

        pedidos = pedidos
            .Where(p => p.PedidoStatus == PedidoStatus.Pago || p.PedidoStatus == PedidoStatus.Cancelado)
            .OrderByDescending(p => p.Codigo);

        if (!pedidos.Any()) return [];

        var pedidoView = new List<PedidoViewModel>();

        foreach (var pedido in pedidos)
        {
            pedidoView.Add(new PedidoViewModel
            {
                Id = pedido.Id,
                ValorTotal = pedido.ValorTotal,
                PedidoStatus = (int)pedido.PedidoStatus,
                Codigo = pedido.Codigo,
                DataCadastro = pedido.DataCadastro,
            });
        }

        return pedidoView;
    }
}