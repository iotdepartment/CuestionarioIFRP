using Microsoft.EntityFrameworkCore;

namespace CuestionarioIFRP.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<CuestionarioIFRP> CuestionarioIFRP { get; set; }
        public DbSet<Pregunta4> Pregunta4 { get; set; }

    }
}
