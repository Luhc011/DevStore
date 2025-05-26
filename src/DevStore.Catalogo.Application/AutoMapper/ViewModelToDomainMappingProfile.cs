using AutoMapper;
using DevStore.Catalogo.Application.ViewModels;
using DevStore.Catalogo.Domain;
using DevStore.Core.DomainObjects;

namespace DevStore.Catalogo.Application.AutoMapper;

public class ViewModelToDomainMappingProfile : Profile
{
    public ViewModelToDomainMappingProfile()
    {
        CreateMap<ProdutoViewModel, Produto>()
            .ConstructUsing(p =>
                new Produto(p.Nome, p.Descricao, p.Ativo,
                    p.Valor, p.CategoriaId, p.DataCadastro,
                    p.Imagem, new Dimensoes(p.Altura, p.Largura, p.Profundidade)));

        CreateMap<CategoriaViewModel, Categoria>()
            .ConstructUsing(c => new Categoria(c.Nome, c.Codigo));
    }
}
