using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetDevPack.Security.Jwt.Core.Model;
using NetDevPack.Security.Jwt.Store.EntityFrameworkCore;
using NSE.Identidade.Models;

namespace NSE.Identidade.Data;

public class ApplicationDbContext : IdentityDbContext, ISecurityKeyContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        SecurityKeys = Set<KeyMaterial>();
        RefreshTokens = Set<RefreshToken>();
    }

    public DbSet<KeyMaterial> SecurityKeys { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}
