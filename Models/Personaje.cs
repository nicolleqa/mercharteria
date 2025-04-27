using System.ComponentModel.DataAnnotations;

namespace mercharteria.Models
{
    public class Personaje
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Nombre { get; set; }

        [Required]
        public string? Imagen { get; set; }

        public virtual ICollection<Producto>? Productos { get; set; }
    }
}