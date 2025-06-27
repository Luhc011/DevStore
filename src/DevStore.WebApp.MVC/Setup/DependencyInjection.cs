using DevStore.Catalogo.Application.Services;
using DevStore.Catalogo.Data;
using DevStore.Catalogo.Data.Repository;
using DevStore.Catalogo.Domain;
using DevStore.Catalogo.Domain.Events;
using DevStore.Core.Communication.Mediator;
using DevStore.Core.Messages.CommonMessages.IntegrationEvents;
using DevStore.Core.Messages.CommonMessages.Notifications;
using DevStore.Pagamentos.AntiCorruption;
using DevStore.Pagamentos.AntiCorruption.Interfaces;
using DevStore.Pagamentos.Business;
using DevStore.Pagamentos.Business.Events;
using DevStore.Pagamentos.Business.Interfaces;
using DevStore.Pagamentos.Data;
using DevStore.Pagamentos.Data.Repository;
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

        services.AddScoped<INotificationHandler<ProdutoAbaixoEstoqueEvent>, ProdutoEventHandler>();
        services.AddScoped<INotificationHandler<PedidoIniciadoEvent>, ProdutoEventHandler>();
        services.AddScoped<INotificationHandler<PedidoProcessamentoCanceladoEvent>, ProdutoEventHandler>();

        // Vendas
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<IPedidoQueries, PedidoQueries>();
        services.AddScoped<VendasContext>();

        services.AddScoped<IRequestHandler<AdicionarItemPedidoCommand, bool>, PedidoCommandHandler>();
        services.AddScoped<IRequestHandler<AtualizarItemPedidoCommand, bool>, PedidoCommandHandler>();
        services.AddScoped<IRequestHandler<RemoverItemPedidoCommand, bool>, PedidoCommandHandler>();
        services.AddScoped<IRequestHandler<AplicarVoucherPedidoCommand, bool>, PedidoCommandHandler>();
        services.AddScoped<IRequestHandler<IniciarPedidoCommand, bool>, PedidoCommandHandler>();
        services.AddScoped<IRequestHandler<FinalizarPedidoCommand, bool>, PedidoCommandHandler>();
        services.AddScoped<IRequestHandler<CancelarProcessamentoPedidoCommand, bool>, PedidoCommandHandler>();
        services.AddScoped<IRequestHandler<CancelarProcessamentoPedidoEstornarEstoqueCommand, bool>, PedidoCommandHandler>();


        services.AddScoped<INotificationHandler<PedidoRascunhoIniciadoEvent>, PedidoEventHandler>();
        services.AddScoped<INotificationHandler<PedidoItemAdicionadoEvent>, PedidoEventHandler>();
        services.AddScoped<INotificationHandler<PedidoEstoqueRejeitadoEvent>, PedidoEventHandler>();
        services.AddScoped<INotificationHandler<PedidoPagamentoRealizadoEvent>, PedidoEventHandler>();
        services.AddScoped<INotificationHandler<PedidoPagamentoRecusadoEvent>, PedidoEventHandler>();

        // Pagamento
        services.AddScoped<IPagamentoRepository, PagamentoRepository>();
        services.AddScoped<IPagamentoService, PagamentoService>();
        services.AddScoped<IPagamentoCartaoCreditoFacade, PagamentoCartaoCreditoFacade>();
        services.AddScoped<IPayPalGateway, PayPalGateway>();
        services.AddScoped<IConfigManager, ConfigManager>();
        services.AddScoped<PagamentoContext>();

        services.AddScoped<INotificationHandler<PedidoEstoqueConfirmadoEvent>, PagamentoEventHandler>();
    }
}
