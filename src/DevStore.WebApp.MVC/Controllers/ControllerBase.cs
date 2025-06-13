using DevStore.Core.Communication.Mediator;
using DevStore.Core.Messages.CommonMessages.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevStore.WebApp.MVC.Controllers;

public abstract class ControllerBase : Controller
{
    private readonly DomainNotificationHandler _notifications;
    private readonly IMediatorHandler _mediatorHandler;
    protected Guid ClienteId => Guid.Parse("4885e451-b0e4-4490-b959-04fabc806d32"); // Simulate a logged-in user for demonstration purposes

    protected ControllerBase(INotificationHandler<DomainNotification> notifications,
                                IMediatorHandler mediatorHandler)
    {
        _notifications = (DomainNotificationHandler)notifications;
        _mediatorHandler = mediatorHandler;
    }

    protected bool OperacaoValida()
    {
        return !_notifications.TemNotificacao();
    }

    protected void NotificarErro(string mensagem)
    {
        _mediatorHandler.PublicarNotificacao(new DomainNotification("Erro", mensagem));
    }

    protected IEnumerable<string> ObterMensagensErro()
    {
        return _notifications.ObterNotificacoes().Select(n => n.Value).ToList();
    }

    protected void NotificarErro(string codigo, string mensagem)
    {
        _mediatorHandler.PublicarNotificacao(new DomainNotification(codigo, mensagem));
    }
}
