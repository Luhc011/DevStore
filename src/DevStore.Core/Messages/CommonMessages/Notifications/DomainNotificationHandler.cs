﻿using MediatR;

namespace DevStore.Core.Messages.CommonMessages.Notifications;

public class DomainNotificationHandler : INotificationHandler<DomainNotification>
{
    private List<DomainNotification> _notifications;

    public DomainNotificationHandler()
    {
        _notifications = [];
    }

    public Task Handle(DomainNotification notification, CancellationToken cancellationToken)
    {
        _notifications.Add(notification);
        return Task.CompletedTask;
    }

    public virtual List<DomainNotification> ObterNotificacoes()
    {
        return _notifications;
    }

    public virtual bool TemNotificacao()
    {
        return ObterNotificacoes().Any();
    }

    public void Dispose()
    {
        _notifications = [];
    }
}