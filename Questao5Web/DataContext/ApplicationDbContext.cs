using Microsoft.EntityFrameworkCore;

namespace Questao5Web.DataContext
{ 

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ContaCorrente> ContaCorrentes { get; set; }
        public DbSet<Movimento> Movimentos { get; set; }
        public DbSet<Idempotencia> Idempotencias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<ContaCorrente>().HasData(
                new ContaCorrente { IdContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9", Numero = 123, Nome = "Katherine Sanchez", Ativo = true },
                new ContaCorrente { IdContaCorrente = "FA99D033-7067-ED11-96C6-7C5DFA4A16C9", Numero = 456, Nome = "Eva Woodward", Ativo = true },
                new ContaCorrente { IdContaCorrente = "382D323D-7067-ED11-8866-7D5DFA4A16C9", Numero = 789, Nome = "Tevin Mcconnell", Ativo = true },
                new ContaCorrente { IdContaCorrente = "F475F943-7067-ED11-A06B-7E5DFA4A16C9", Numero = 741, Nome = "Ameena Lynn", Ativo = false },
                new ContaCorrente { IdContaCorrente = "BCDACA4A-7067-ED11-AF81-825DFA4A16C9", Numero = 852, Nome = "Jarrad Mckee", Ativo = false },
                new ContaCorrente { IdContaCorrente = "D2E02051-7067-ED11-94C0-835DFA4A16C9", Numero = 963, Nome = "Elisha Simons", Ativo = false }
            );
        }
    }

}
