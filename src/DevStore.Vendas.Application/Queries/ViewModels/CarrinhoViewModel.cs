namespace DevStore.Vendas.Application.Queries.ViewModels;

public class CarrinhoViewModel
{
    public Guid PedidoId { get; set; }
    public Guid ClienteId { get; set; }
    public decimal SubTotal { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal ValorDesconto { get; set; }
    public string VoucherCodigo { get; set; } = string.Empty;

    public List<CarrinhoItemViewModel> Items { get; set; } = [];
    public CarrinhoPagamentoViewModel Pagamento { get; set; } = new();
}
