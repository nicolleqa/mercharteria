using System.ComponentModel.DataAnnotations;

namespace mercharteria.Models
{
    public class Testimonio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Nombre { get; set; }

        [Required]
        [StringLength(500)]
        public string? Comentario { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
    }
}