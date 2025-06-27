using DevStore.Core.Data;

namespace DevStore.Pagamentos.Business.Interfaces;

public interface IPagamentoRepository : IRepository<Pagamento>
{
    void Adicionar(Pagamento pagamento);

    void AdicionarTransacao(Transacao transacao);
}
