using DevStore.Catalogo.Application.ViewModels;

namespace DevStore.Catalogo.Application.Services;

public interface IProdutoAppService : IDisposable
{
    Task<IEnumerable<ProdutoViewModel>> ObterPorCategoria(int codigo);
    Task<ProdutoViewModel> ObterPorId(Guid id);
    Task<IEnumerable<ProdutoViewModel>> ObterTodos();
    Task<IEnumerable<CategoriaViewModel>> ObterCategorias();

    Task AdicionarProduto(ProdutoViewModel produtoViewModel);
    Task AtualizarProduto(ProdutoViewModel produtoViewModel);

    Task<ProdutoViewModel> DebitarEstoque(Guid id, int quantidade);
    Task<ProdutoViewModel> ReporEstoque(Guid id, int quantidade);
}
