using ApiCatalago.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalago.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Categoria>? Categorias { get; set; }   
    public DbSet<Produto>? Produtos { get; set; }

    internal object Find(int prdId)
    {
        throw new NotImplementedException();
    }
}
