using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PracticaGestionInventario.Models;

namespace PracticaGestionInventario.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Entradas> Entradas { get; set; }
        public DbSet<EntradasDetalles> EntradasDetalles { get; set; }
        public DbSet<Productos> Productos { get; set; }
    }
}
