using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using mercharteria.Models;
using System.ComponentModel.DataAnnotations.Schema;


namespace mercharteria.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Producto> DbSetProducto { get; set; }
    public DbSet<PreOrden> DbSetPreOrden { get; set; }
    public DbSet<Orden> DbSetOrden { get; set; }
    public DbSet<Pago> DbSetPago { get; set; }

    public DbSet<Testimonio> Testimonios { get; set; }
    public DbSet<Personaje> Personajes { get; set; }
}




