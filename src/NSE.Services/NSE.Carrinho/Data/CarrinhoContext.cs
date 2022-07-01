using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using NSE.Carrinho.Model;

namespace NSE.Carrinho.Data;

public class CarrinhoContext : DbContext
{
    public CarrinhoContext(DbContextOptions<CarrinhoContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
        CarrinhoItens = Set<CarrinhoItem>();
        CarrinhoCliente = Set<CarrinhoCliente>();
    }

    public DbSet<CarrinhoItem> CarrinhoItens { get; set; }
    public DbSet<CarrinhoCliente> CarrinhoCliente { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<ValidationResult>();

        foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(100)");

        modelBuilder.Entity<CarrinhoCliente>()
            .HasIndex(c => c.ClienteId)
            .HasDatabaseName("IDX_Cliente");

        modelBuilder.Entity<CarrinhoCliente>()
            .HasMany(c => c.Itens)
            .WithOne(i => i.CarrinhoCliente);

        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
    }
}
