using Microsoft.EntityFrameworkCore;

namespace CuestionarioIFRP.Models
{
    public class RhDbContext : DbContext
    {
        public RhDbContext(DbContextOptions<RhDbContext> options) : base(options)
        {
        }

        // Tu tabla de empleados de la otra base de datos
        public DbSet<rh4> rh4 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Si la tabla física se llama diferente o usa un esquema (ej: "dbo.rh4"), lo especificas aquí:
            modelBuilder.Entity<rh4>().ToTable("rh4");
        }
    }
}
