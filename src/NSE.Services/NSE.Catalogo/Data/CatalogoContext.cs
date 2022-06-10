using System.ComponentModel.DataAnnotations;
using Core.Data;
using Core.Messages;
using Microsoft.EntityFrameworkCore;
using NSE.Catalogo.Models;

namespace NSE.Catalogo.Data;

public class CatalogoContext : DbContext, IUnitOfWork
{
    public DbSet<Produto> Produtos { get; set; }

    public CatalogoContext(DbContextOptions<CatalogoContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<ValidationResult>();
        modelBuilder.Ignore<Event>();

        base.OnModelCreating(modelBuilder);

        foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetProperties()
                .Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(100)");

        modelBuilder.ApplyConfigurationsFromAssembly(assembly: GetType().Assembly);
    }

    public async Task<bool> Commit()
        => await base.SaveChangesAsync() > 0;
}
