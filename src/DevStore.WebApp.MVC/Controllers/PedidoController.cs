using DevStore.Core.Communication.Mediator;
using DevStore.Core.Messages.CommonMessages.Notifications;
using DevStore.Vendas.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevStore.WebApp.MVC.Controllers;

public class PedidoController : ControllerBase
{
    private readonly IPedidoQueries _pedidoQueries;
    public PedidoController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediatorHandler, IPedidoQueries pedidoQueries) : base(notifications, mediatorHandler)
    {
        _pedidoQueries = pedidoQueries;
    }

    [Route("meus-pedidos")]
    public async Task<IActionResult> Index()
    {
        return View(await _pedidoQueries.ObterPedidosCliente(ClienteId));
    }
}
