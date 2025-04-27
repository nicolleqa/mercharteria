using System.ComponentModel.DataAnnotations;

namespace mercharteria.Models
{
    public class Contacto
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        [Phone]
        public string NumeroTelefono { get; set; }

        public string Mensaje { get; set; }
    }
}
