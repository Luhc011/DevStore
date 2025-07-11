﻿using AutoMapper;
using DevStore.Catalogo.Application.ViewModels;
using DevStore.Catalogo.Domain;
using DevStore.Core.DomainObjects;

namespace DevStore.Catalogo.Application.Services;

public class ProdutoAppService : IProdutoAppService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IEstoqueService _estoqueService;
    private readonly IMapper _mapper;

    public ProdutoAppService(IProdutoRepository produtoRepository, IMapper mapper, IEstoqueService estoqueService)
    {
        _produtoRepository = produtoRepository;
        _mapper = mapper;
        _estoqueService = estoqueService;
    }
    public async Task<IEnumerable<ProdutoViewModel>> ObterPorCategoria(int codigo)
    {
        return _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterPorCategoria(codigo));
    }

    public async Task<ProdutoViewModel> ObterPorId(Guid id)
    {
        return _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterPorId(id));
    }

    public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
    {
        return _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterTodos());
    }

    public async Task<IEnumerable<CategoriaViewModel>> ObterCategorias()
    {
        return _mapper.Map<IEnumerable<CategoriaViewModel>>(await _produtoRepository.ObterCategorias());
    }

    public async Task AdicionarProduto(ProdutoViewModel produtoViewModel)
    {
        var produto = _mapper.Map<Produto>(produtoViewModel);
        _produtoRepository.Adicionar(produto);

        await _produtoRepository.UnitOfWork.Commit();
    }

    public async Task AtualizarProduto(ProdutoViewModel produtoViewModel)
    {
        var produto = _mapper.Map<Produto>(produtoViewModel);
        _produtoRepository.Adicionar(produto);

        await _produtoRepository.UnitOfWork.Commit();
    }

    public async Task<ProdutoViewModel> DebitarEstoque(Guid id, int quantidade)
    {
        var possuiEstoque = await _estoqueService.DebitarEstoque(id, quantidade);

        if (!possuiEstoque) throw new DomainException("Falha ao debitar");

        return _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterPorId(id));
    }

    public async Task<ProdutoViewModel> ReporEstoque(Guid id, int quantidade)
    {
        if (!_estoqueService.ReporEstoque(id, quantidade).Result) throw new DomainException("Falha ao repor estoque");

        return _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterPorId(id));
    }
    public void Dispose()
    {
        _produtoRepository?.Dispose();
        _estoqueService?.Dispose();
    }
}