using DevStore.Catalogo.Application.Services;
using DevStore.Catalogo.Data;
using DevStore.Catalogo.Data.Repository;
using DevStore.Catalogo.Domain;
using DevStore.Core.Communication.Mediator;
using DevStore.Core.Messages.CommonMessages.Notifications;
using DevStore.Vendas.Application.Commands;
using DevStore.Vendas.Application.Events;
using DevStore.Vendas.Application.Queries;
using DevStore.Vendas.Data;
using DevStore.Vendas.Data.Repository;
using DevStore.Vendas.Domain;
using MediatR;

namespace DevStore.WebApp.MVC.Setup;

public static class DependencyInjection
{
    public static void AddRegisterServices(this IServiceCollection services)
    {
        // MediatR
        services.AddScoped<IMediatorHandler, MediatorHandler>();

        // Domain Notifications
        services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

        // Catalogo
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IProdutoAppService, ProdutoAppService>();
        services.AddScoped<IEstoqueService, EstoqueService>();
        services.AddScoped<CatalogoContext>();

        //services.AddScoped<INotificationHandler<ProdutoAbaixoEstoqueEvent>, ProdutoEventHandler>();

        // Vendas
        services.AddScoped<IRequestHandler<AdicionarItemPedidoCommand, bool>, PedidoCommandHandler>();
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<IPedidoQueries, PedidoQueries>();
        services.AddScoped<VendasContext>();

        services.AddScoped<INotificationHandler<PedidoRascunhoIniciadoEvent>, PedidoEventHandler>();
        services.AddScoped<INotificationHandler<PedidoItemAdicionadoEvent>, PedidoEventHandler>();

    }
}
