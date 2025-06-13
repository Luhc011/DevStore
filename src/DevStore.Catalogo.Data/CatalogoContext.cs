using DevStore.Catalogo.Domain;
using DevStore.Core.Data;
using DevStore.Core.Messages;
using Microsoft.EntityFrameworkCore;

namespace DevStore.Catalogo.Data;

public class CatalogoContext : DbContext, IUnitOfWork
{
    public CatalogoContext(DbContextOptions<CatalogoContext> options) : base(options) { }

    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Categoria> Categorias { get; set; }

    public async Task<bool> Commit()
    {
        var entityEntries = ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null);

        foreach (var entry in entityEntries)
        {
            if (entry.State is EntityState.Added) entry.Property("DataCadastro").CurrentValue = DateTime.Now;
            if (entry.State is EntityState.Modified) entry.Property("DataCadastro").IsModified = false;
        }

        return await base.SaveChangesAsync() > 0;
    }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        foreach (var property in mb.Model.GetEntityTypes().SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
        {
            property.SetColumnType("varchar(100)");
        }

        mb.Ignore<Event>();
        mb.ApplyConfigurationsFromAssembly(typeof(CatalogoContext).Assembly);
    }
}
