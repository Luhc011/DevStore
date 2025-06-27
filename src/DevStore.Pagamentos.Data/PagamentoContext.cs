using DevStore.Core.Communication.Mediator;
using DevStore.Core.Data;
using DevStore.Core.Messages;
using DevStore.Pagamentos.Business;
using Microsoft.EntityFrameworkCore;

namespace DevStore.Pagamentos.Data;

public class PagamentoContext : DbContext, IUnitOfWork
{
    private readonly IMediatorHandler _mediator;

    public PagamentoContext(DbContextOptions<PagamentoContext> options, IMediatorHandler mediator) : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<Pagamento> Pagamentos { get; set; }
    public DbSet<Transacao> Transacoes { get; set; }

    public async Task<bool> Commit()
    {
        var entityEntries = ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null);

        foreach (var entry in entityEntries)
        {
            if (entry.State is EntityState.Added) entry.Property("DataCadastro").CurrentValue = DateTime.Now;
            if (entry.State is EntityState.Modified) entry.Property("DataCadastro").IsModified = false;
        }

        var sucesso = await base.SaveChangesAsync() > 0;
        if (sucesso) await _mediator.PublicarEventos(this);

        return sucesso;
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

        foreach (var property in decimalProps)
        {
            property.SetColumnType("decimal(18,2)");
        }

        mb.Ignore<Event>();

        mb.ApplyConfigurationsFromAssembly(typeof(PagamentoContext).Assembly);

        foreach (var relationship in mb.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
        }

        base.OnModelCreating(mb);
    }
}

