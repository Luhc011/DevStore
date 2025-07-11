﻿using DevStore.Core.Messages;

namespace DevStore.Core.DomainObjects;

public abstract class Entity
{
    public Guid Id { get; set; }
    private List<Event> _notificacoes;
    public IReadOnlyCollection<Event> Notificacoes => _notificacoes?.AsReadOnly();

    protected Entity()
    {
        Id = Guid.NewGuid();
    }
    public void AdicionarEvento(Event evento)
    {
        _notificacoes = _notificacoes ?? [];
        _notificacoes.Add(evento);
    }

    public void RemoverEvento(Event eventItem)
    {
        _notificacoes?.Remove(eventItem);
    }

    public void LimparEventos()
    {
        _notificacoes?.Clear();
    }
    public static bool operator ==(Entity a, Entity b)
    {
        if (a is null && b is null) return true;
        if (a is null || b is null) return false;

        return a.Equals(b);
    }
    
    public static bool operator !=(Entity a, Entity b) => !(a == b);

    public override bool Equals(object? obj)
    {
        var compareTo = obj as Entity;

        if (ReferenceEquals(this, compareTo)) return true;
        if (compareTo is null) return false;

        return Id.Equals(compareTo.Id);
    }
    public override int GetHashCode() => (GetType().GetHashCode() * 907 + Id.GetHashCode());
    public override string ToString() => $"{GetType().Name} [Id={Id}]";
    public virtual bool EhValido()
    {
        throw new NotImplementedException();
    }
}
