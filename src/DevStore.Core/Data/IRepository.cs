using DevStore.Core.DomainObjects;

namespace DevStore.Core.Data;

public interface IRepository<T> : IDisposable where T : IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
}

