using DevStore.Catalogo.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevStore.WebApp.MVC.Controllers;

public class VitrineController : Controller
{
    private readonly IProdutoAppService _produtoAppService;

    public VitrineController(IProdutoAppService produtoAppService)
    {
        _produtoAppService = produtoAppService;
    }

    [HttpGet("")]
    [Route("vitrine")]
    public async Task<IActionResult> Index()
    {
        return View(await _produtoAppService.ObterTodos());
    }

    [HttpGet("produto-detalhe/{id}")]
    public async Task<IActionResult> ProdutoDetalhe(Guid id)
    {
        return View(await _produtoAppService.ObterPorId(id));
    }
}
