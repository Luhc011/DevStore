﻿using DevStore.Catalogo.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevStore.Catalogo.Data.Mappings;

public class ProdutoMapping : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome).IsRequired().HasColumnType("varchar(250)");
        builder.Property(c => c.Descricao).IsRequired().HasColumnType("varchar(500)");
        builder.Property(c => c.Imagem).IsRequired().HasColumnType("varchar(250)");

        builder.OwnsOne(c => c.Dimensoes, cm => //transforma o objeto dimensoes em colunas na tabela de produto
        {
            cm.Property(c => c.Altura)
                    .HasColumnName("Altura")
                    .HasColumnType("int");

            cm.Property(c => c.Largura)
                .HasColumnName("Largura")
                .HasColumnType("int");

            cm.Property(c => c.Profundidade)
                .HasColumnName("Profundidade")
                .HasColumnType("int");
        });

        builder.ToTable("Produtos");
    }
}
