using DevStore.Catalogo.Application.Services;
using DevStore.Catalogo.Data;
using DevStore.Catalogo.Data.Repository;
using DevStore.Catalogo.Domain;
using DevStore.Catalogo.Domain.Events;
using DevStore.Core.Communication.Mediator;
using DevStore.Vendas.Application.Commands;
using MediatR;

namespace DevStore.WebApp.MVC.Setup;

public static class DependencyInjection
{
    public static void AddRegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IMediatorHandler, MediatorHandler>();

        // Catalogo
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IProdutoAppService, ProdutoAppService>();
        services.AddScoped<IEstoqueService, EstoqueService>();
        services.AddScoped<CatalogoContext>();

        //services.AddScoped<INotificationHandler<ProdutoAbaixoEstoqueEvent>, ProdutoEventHandler>();

        // Vendas
        services.AddScoped<IRequestHandler<AdicionarItemPedidoCommand, bool>, PedidoCommandHandler>()
    }
}
