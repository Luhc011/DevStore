using DevStore.Catalogo.Application.Services;
using DevStore.Core.Communication.Mediator;
using DevStore.Vendas.Application.Commands;
using Microsoft.AspNetCore.Mvc;

namespace DevStore.WebApp.MVC.Controllers;

public class CarrinhoController : ControllerBase
{
    private readonly IProdutoAppService _produtoAppService;
    private readonly IMediatorHandler _mediatorHandler;

    public CarrinhoController(IProdutoAppService produtoAppService, IMediatorHandler mediatorHandler)
    {
        _produtoAppService = produtoAppService;
        _mediatorHandler = mediatorHandler;
    }

    [Route("meu-carrinho")]
    public async Task<IActionResult> Index()
    {
        await Task.Delay(0);
        return View();
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


        TempData["Erro"] = "Produto indisponivel";
        return RedirectToAction("ProdutoDetalhe", "Vitrine", new { id });
    }

}

public abstract class ControllerBase : Controller
{
    protected Guid ClienteId => Guid.NewGuid(); // Simulate a logged-in user for demonstration purposes
}
