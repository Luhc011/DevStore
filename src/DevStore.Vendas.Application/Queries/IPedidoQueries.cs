using DevStore.Vendas.Application.Queries.ViewModels;

namespace DevStore.Vendas.Application.Queries;

public interface IPedidoQueries
{
    Task<CarrinhoViewModel> ObterCarrinhoCliente(Guid clienteId);
    Task<IEnumerable<PedidoViewModel>> ObterPedidosCliente(Guid clienteId);
}
