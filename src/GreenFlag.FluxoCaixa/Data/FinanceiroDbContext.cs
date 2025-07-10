using GreenFlag.Financeiro.Core.Lancamentos.Entities;
using Microsoft.EntityFrameworkCore;

namespace GreenFlag.Financeiro.Api.Data
{
    public class FinanceiroDbContext : DbContext
    {
        public DbSet<Lancamento> Lancamentos { get; set; }

        public FinanceiroDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
