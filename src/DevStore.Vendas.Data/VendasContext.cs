using DevStore.Core.Communication.Mediator;
using DevStore.Core.Data;
using DevStore.Core.Messages;
using DevStore.Vendas.Domain;
using Microsoft.EntityFrameworkCore;

namespace DevStore.Vendas.Data;

public class VendasContext : DbContext, IUnitOfWork
{
    private readonly IMediatorHandler _mediator;
    public VendasContext(DbContextOptions<VendasContext> options, IMediatorHandler mediator) : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<PedidoItem> PedidoItems { get; set; }
    public DbSet<Voucher> Vouchers { get; set; }


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

        var decimalProps = mb.Model
            .GetEntityTypes()
            .SelectMany(e => e.GetProperties())
            .Where(p => p.ClrType == typeof(decimal));

        foreach(var property in decimalProps)
        {
            property.SetColumnType("decimal(18,2)");
        }

        mb.Ignore<Event>();

        mb.ApplyConfigurationsFromAssembly(typeof(VendasContext).Assembly);

        foreach (var relationship in mb.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
        }

        mb.HasSequence<int>("MinhaSequencia")
            .StartsAt(1000)
            .IncrementsBy(1);

        base.OnModelCreating(mb);
    }
}
