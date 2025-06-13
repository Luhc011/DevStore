namespace DevStore.Vendas.Application.Queries.ViewModels;

public class CarrinhoItemViewModel
{
    public Guid ProdutoId { get; set; }
    public string ProdutoNome { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal ValorTotal { get; set; }

}