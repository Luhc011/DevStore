﻿using DevStore.Catalogo.Domain.Events;
using DevStore.Core.Communication.Mediator;
using DevStore.Core.DomainObjects.DTO;
using DevStore.Core.Messages.CommonMessages.Notifications;

namespace DevStore.Catalogo.Domain;
public class EstoqueService : IEstoqueService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IMediatorHandler _mediatorHandler;

    public EstoqueService(IProdutoRepository produtoRepository, IMediatorHandler mediatorHandler)
    {
        _produtoRepository = produtoRepository;
        _mediatorHandler = mediatorHandler;
    }

    public async Task<bool> DebitarEstoque(Guid produtoId, int quantidade)
    {
        if (!await DebitarItemEstoque(produtoId, quantidade)) return false;

        return await _produtoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> DebitarListaProdutosPedido(ListaProdutosPedido lista)
    {
        foreach (var item in lista.Itens)
        {
            if (!await DebitarItemEstoque(item.Id, item.Quantidade)) return false;
        }

        return await _produtoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> ReporListaProdutosPedido(ListaProdutosPedido lista)
    {
        foreach (var item in lista.Itens)
        {
            await ReporItemEstoque(item.Id, item.Quantidade);
        }

        return await _produtoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> ReporEstoque(Guid produtoId, int quantidade)
    {
        var sucesso = await ReporItemEstoque(produtoId, quantidade);

        if (!sucesso) return false;

        return await _produtoRepository.UnitOfWork.Commit();
    }

    private async Task<bool> DebitarItemEstoque(Guid produtoId, int quantidade)
    {
        var produto = await _produtoRepository.ObterPorId(produtoId);

        if (produto is null) return false;

        if (!produto.PossuiEstoque(quantidade))
        {
            await _mediatorHandler.PublicarNotificacao(new DomainNotification("Estoque", $"Produto - {produto.Nome} sem estoque"));
            return false;
        }

        produto.DebitarEstoque(quantidade);

        // TODO: 10 pode ser parametrizavel em arquivo de configuração
        if (produto.QuantidadeEstoque < 10)
        {
            await _mediatorHandler.PublicarDomainEvent(new ProdutoAbaixoEstoqueEvent(produto.Id, produto.QuantidadeEstoque));
        }

        _produtoRepository.Atualizar(produto);
        return true;
    }

    private async Task<bool> ReporItemEstoque(Guid produtoId, int quantidade)
    {
        var produto = await _produtoRepository.ObterPorId(produtoId);

        if (produto is null) return false;
        produto.ReporEstoque(quantidade);

        _produtoRepository.Atualizar(produto);

        return true;
    }

    public void Dispose()
    {
        _produtoRepository.Dispose();
    }
}
