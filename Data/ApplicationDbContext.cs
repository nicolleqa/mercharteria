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
    public DbSet<Producto> Productos { get; set; }
    public DbSet<PreOrden> DbSetPreOrden { get; set; }
    public DbSet<Orden> DbSetOrden { get; set; }
    public DbSet<Pago> DbSetPago { get; set; }
    public DbSet<DetalleOrden> DbSetDetalleOrden { get; set; }
    public DbSet<DatosCliente> DatosClientes { get; set; }
    public DbSet<Testimonio> Testimonios { get; set; }
    public DbSet<Personaje> Personajes { get; set; }

    public DbSet<Categoria> Categorias { get; set; }

    public DbSet<Contacto> Contactos { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configuración de la relación muchos a muchos entre Producto y Personaje
        modelBuilder.Entity<Producto>()
            .HasMany(p => p.Personajes)
            .WithMany(p => p.Productos);

        // Configuración de la relación uno a muchos entre Categoria y Producto
        modelBuilder.Entity<Producto>()
            .HasOne(p => p.Categoria)
            .WithMany(c => c.Productos)
            .HasForeignKey(p => p.CategoriaId);
    }
}




