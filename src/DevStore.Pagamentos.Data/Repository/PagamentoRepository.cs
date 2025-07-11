﻿using DevStore.Core.Data;
using DevStore.Pagamentos.Business;
using DevStore.Pagamentos.Business.Interfaces;

namespace DevStore.Pagamentos.Data.Repository;

public class PagamentoRepository : IPagamentoRepository
{
    private readonly PagamentoContext _context;

    public PagamentoRepository(PagamentoContext context)
    {
        _context = context;
    }
    public IUnitOfWork UnitOfWork => _context;

    public void Adicionar(Pagamento pagamento)
    {
        _context.Pagamentos.Add(pagamento);
    }

    public void AdicionarTransacao(Transacao transacao)
    {
        _context.Transacoes.Add(transacao);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
