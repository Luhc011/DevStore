using DevStore.Catalogo.Application.Services;
using DevStore.Core.Communication.Mediator;
using DevStore.Core.Messages.CommonMessages.Notifications;
using DevStore.Vendas.Application.Commands;
using DevStore.Vendas.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevStore.WebApp.MVC.Controllers;

public class CarrinhoController : ControllerBase
{
    private readonly IProdutoAppService _produtoAppService;
    private readonly IPedidoQueries _pedidoQueries;
    private readonly IMediatorHandler _mediatorHandler;

    public CarrinhoController(IProdutoAppService produtoAppService, IMediatorHandler mediatorHandler, INotificationHandler<DomainNotification> notifications, IPedidoQueries pedidoQueries) : base(notifications, mediatorHandler)
    {
        _produtoAppService = produtoAppService;
        _mediatorHandler = mediatorHandler;
        _pedidoQueries = pedidoQueries;
    }

    [Route("meu-carrinho")]
    public async Task<IActionResult> Index()
    {
        return View(await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
    }

    [HttpPost("meu-carrinho")]
    public async Task<IActionResult> AdicionarItem(Guid id, int quantidade)
    {
        var produto = await _produtoAppService.ObterPorId(id);
        if (produto is null) return BadRequest();

        if (produto.QuantidadeEstoque < quantidade)
        {
            TempData["Erro"] = "Produto com estoque insuficiente";
            return RedirectToAction("ProdutoDetalhe", "Vitrine", new { id });
        }

        var command = new AdicionarItemPedidoCommand(ClienteId, produto.Id, produto.Nome, quantidade, produto.Valor);

        await _mediatorHandler.EnviarComando(command);

        if (OperacaoValida()) return RedirectToAction("Index");

        TempData["Erros"] = ObterMensagensErro();
        return RedirectToAction("ProdutoDetalhe", "Vitrine", new { id });
    }

}
