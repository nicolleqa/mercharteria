using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;


namespace mercharteria.Models
{
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [NotNull]
        public string? Nombre { get; set; }
        [NotNull]
        public string? Descripcion { get; set; }
        [NotNull]
        public decimal Precio { get; set; }
        [NotNull]
        public string? ImagenUrl { get; set; }

        // Relación con Categoria
        public int CategoriaId { get; set; }
        public virtual Categoria? Categoria { get; set; }

        // Relación muchos a muchos con Personajes
        public virtual ICollection<Personaje>? Personajes { get; set; }
        
    }
}