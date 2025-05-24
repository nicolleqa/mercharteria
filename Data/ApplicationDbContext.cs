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
    public DbSet<ProductoPersonaje> ProductoPersonajes { get; set; }

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

        // 1 Orden tiene muchos DetalleOrden
        modelBuilder.Entity<Orden>()
            .HasMany(o => o.Detalles)
            .WithOne(d => d.Orden!)
            .HasForeignKey(d => d.OrdenId);

        // 1 Orden tiene un Pago opcional
        modelBuilder.Entity<Orden>()
            .HasOne(o => o.Pago)
            .WithMany()
            .HasForeignKey(o => o.PagoId);

        // 1 Orden tiene un DatosCliente opcional
        modelBuilder.Entity<Orden>()
            .HasOne(o => o.DatosCliente)
            .WithMany()
            ;    
            
        modelBuilder.Entity<ProductoPersonaje>()
        .HasKey(pp => new { pp.ProductoId, pp.PersonajeId });

        modelBuilder.Entity<ProductoPersonaje>()
            .HasOne(pp => pp.Producto)
            .WithMany()
            .HasForeignKey(pp => pp.ProductoId);

        modelBuilder.Entity<ProductoPersonaje>()
            .HasOne(pp => pp.Personaje)
            .WithMany()
            .HasForeignKey(pp => pp.PersonajeId);
    }
}




