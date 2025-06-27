using DevStore.Core.DomainObjects;

namespace DevStore.Pagamentos.Business;

public class Pagamento : Entity, IAggregateRoot
{
    public Guid PedidoId { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal Valor { get; set; }

    public string NomeCartao { get; set; } = string.Empty;
    public string NumeroCartao { get; set; } = string.Empty;
    public string ExpiracaoCartao { get; set; } = string.Empty;
    public string CvvCartao { get; set; } = string.Empty;

    // EF. Rel.
    public Transacao Transacao { get; set; } 
}
