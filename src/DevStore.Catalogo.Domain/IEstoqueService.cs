﻿using DevStore.Core.DomainObjects.DTO;

namespace DevStore.Catalogo.Domain;

public interface IEstoqueService : IDisposable
{
    Task<bool> DebitarEstoque(Guid produtoId, int quantidade);
    Task<bool> DebitarListaProdutosPedido(ListaProdutosPedido lista);
    Task<bool> ReporEstoque(Guid produtoId, int quantidade);
    Task<bool> ReporListaProdutosPedido(ListaProdutosPedido lista);
}
