using DevStore.Core.Messages.CommonMessages.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevStore.WebApp.MVC.Extensions;

public class SummaryViewComponent : ViewComponent
{
    private readonly DomainNotificationHandler _notifications;

    public SummaryViewComponent(INotificationHandler<DomainNotification> notifications)
    {
        _notifications = (DomainNotificationHandler)notifications;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var notificacaoes = await Task.FromResult(_notifications.ObterNotificacoes());
        notificacaoes.ForEach(c => ViewData.ModelState.AddModelError(string.Empty, c.Value));

        return View();
    }
}