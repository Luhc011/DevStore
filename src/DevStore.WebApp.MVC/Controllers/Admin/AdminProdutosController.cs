using DevStore.Catalogo.Application.Services;
using DevStore.Catalogo.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DevStore.WebApp.MVC.Controllers.Admin;

public class AdminProdutosController : Controller
{
    private readonly IProdutoAppService _produtoAppService;

    public AdminProdutosController(IProdutoAppService produtoAppService)
    {
        _produtoAppService = produtoAppService;
    }

    [HttpGet("admin-produtos")]
    public async Task<IActionResult> Index()
    {
        return View(await _produtoAppService.ObterTodos());
    }

    [Route("novo-produto")]
    public async Task<IActionResult> NovoProduto()
    {
        return View(await PopularCategorias(new ProdutoViewModel()));
    }

    [HttpPost("novo-produto")]
    public async Task<IActionResult> NovoProduto(ProdutoViewModel produto)
    {
        if (!ModelState.IsValid) return View(await PopularCategorias(produto));

        await _produtoAppService.AdicionarProduto(produto);

        return RedirectToAction("Index");
    }

    [HttpGet("editar-produto")]
    public async Task<IActionResult> AtualizarProduto(Guid id)
    {
        return View(await PopularCategorias(await _produtoAppService.ObterPorId(id)));
    }

    [HttpPost("editar-produto")]
    public async Task<IActionResult> AtualizarProduto(Guid id, ProdutoViewModel produtoViewModel)
    {
        var produto = await _produtoAppService.ObterPorId(id);
        produtoViewModel.QuantidadeEstoque = produto.QuantidadeEstoque;

        ModelState.Remove("QuantidadeEstoque");
        if (!ModelState.IsValid) return View(await PopularCategorias(produtoViewModel));

        await _produtoAppService.AtualizarProduto(produtoViewModel);

        return RedirectToAction("Index");
    }

    [HttpGet("produtos-atualizar-estoque")]
    public async Task<IActionResult> AtualizarEstoque(Guid id)
    {
        return View("Estoque", await _produtoAppService.ObterPorId(id));
    }

    [HttpPost("produtos-atualizar-estoque")]
    public async Task<IActionResult> AtualizarEstoque(Guid id, int quantidade)
    {
        if (quantidade > 0) 
            await _produtoAppService.DebitarEstoque(id, quantidade);
        else 
            await _produtoAppService.ReporEstoque(id, quantidade);

        return View("Index", await _produtoAppService.ObterTodos());
    }


    private async Task<ProdutoViewModel> PopularCategorias(ProdutoViewModel produto)
    {
        produto.Categorias = await _produtoAppService.ObterCategorias();
        return produto;
    }
}
