
using Microsoft.EntityFrameworkCore;
using tech_test_payment_api.Entities;

namespace tech_test_payment_api.Context
{
    public class LojaContext : DbContext
    {
        public LojaContext(DbContextOptions<LojaContext> options) : base(options)
        {}

        public DbSet<Venda> Vendas { get; set;}
        public DbSet<Vendedor> Vendedores { get; set; }
    }
}