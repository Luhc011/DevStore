using DevStore.Core.Communication.Mediator;
using DevStore.Core.DomainObjects;

namespace DevStore.Pagamentos.Data;

public static class MediatorExtension
{
    public static async Task PublicarEventos(this IMediatorHandler mediator, PagamentoContext context)
    {
        var domainEntities = context.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.Notificacoes != null && x.Entity.Notificacoes.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.Notificacoes).ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.LimparEventos());

        var tasks = domainEvents
            .Select(async (domainEvents) =>
            {
                await mediator.PublicarEvento(domainEvents);
            });

        await Task.WhenAll(tasks);
    }
}

