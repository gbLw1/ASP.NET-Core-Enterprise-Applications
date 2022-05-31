using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NSE.Catalogo.Models;

namespace NSE.Catalogo.Data.Mappings;

public class ProdutoMapping : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.ToTable("Produtos");

        builder
            .HasKey(p => p.Id);

        builder
            .Property(p => p.Nome)
            .HasMaxLength(250)
            .IsRequired();

        builder
            .Property(p => p.Descricao)
            .HasMaxLength(500)
            .IsRequired();

        builder
            .Property(p => p.Imagem)
            .HasMaxLength(250)
            .IsRequired();
    }
}
