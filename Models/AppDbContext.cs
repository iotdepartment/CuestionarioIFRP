using Microsoft.EntityFrameworkCore;

namespace CuestionarioIFRP.Models
{
    // Conectado a DefaultConnection (Base de datos RH)
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // MOVIDO AQUÍ: El cuestionario se almacena en la base de datos RH
        public DbSet<CuestionarioIFRP> CuestionarioIFRP { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CuestionarioIFRP>().ToTable("CuestionarioIFRP");
        }
    }

    // Conectado a UserConnection (Base de datos TGRMX)
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        // MOVIDO AQUÍ: Los empleados de rh4 se consultan en la base de datos TGRMX
        public DbSet<rh4> rh4 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<rh4>().ToTable("rh4");
        }
    }
}
