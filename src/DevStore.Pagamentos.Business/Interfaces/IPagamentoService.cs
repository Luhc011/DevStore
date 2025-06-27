using DevStore.Core.DomainObjects.DTO;

namespace DevStore.Pagamentos.Business.Interfaces;

public interface IPagamentoService
{
    Task<Transacao> RealizarPagamentoPedido(PagamentoPedido pagamentoPedido);
}