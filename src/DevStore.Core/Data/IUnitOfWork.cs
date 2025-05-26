namespace DevStore.Core.Data;

public interface IUnitOfWork
{
    Task<bool> Commit();
}

