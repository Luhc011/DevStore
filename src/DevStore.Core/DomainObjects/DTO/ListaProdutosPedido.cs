﻿namespace DevStore.Core.DomainObjects.DTO;

public class ListaProdutosPedido
{
    public Guid PedidoId { get; set; }
    public ICollection<Item> Itens { get; set; } = [];
}

public class Item
{
    public Guid Id { get; set; }
    public int Quantidade { get; set; }
}
