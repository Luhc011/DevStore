using DevStore.Core.Communication.Mediator;
using DevStore.Core.Messages;
using MediatR;

namespace DevStore.Vendas.Application.Commands;

public class PedidoCommandHandler :
    IRequestHandler<AdicionarItemPedidoCommand, bool>
{
    private readonly IMediatorHandler _mediatorHandler;

    public PedidoCommandHandler(IMediatorHandler mediatorHandler)
    {
        _mediatorHandler = mediatorHandler;
    }

    public async Task<bool> Handle(AdicionarItemPedidoCommand message, CancellationToken cancellationToken)
    {
        if (!ValidarComando(message))
        {
            return false;
        }
        return true;
    }

    private bool ValidarComando(Command message)
    {
        if (message.EhValido()) return true;

        foreach (var error in message.ValidationResult.Errors)
        {
            // publish a notification with the error

        }

        return false;
    }
}
