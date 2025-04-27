using System.ComponentModel.DataAnnotations;

namespace mercharteria.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Nombre { get; set; }

        public virtual ICollection<Producto>? Productos { get; set; }
    }
}