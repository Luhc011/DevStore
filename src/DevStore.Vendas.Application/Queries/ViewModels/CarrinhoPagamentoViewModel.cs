namespace DevStore.Vendas.Application.Queries.ViewModels;

public class CarrinhoPagamentoViewModel
{
    public string NomeCartao { get; set; } = string.Empty;
    public string NumeroCartao { get; set; } = string.Empty;
    public string ExpiracaoCartao { get; set; } = string.Empty;
    public string CvvCartao { get; set; } = string.Empty;
}
