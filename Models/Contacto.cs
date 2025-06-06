using System.ComponentModel.DataAnnotations;

namespace mercharteria.Models
{
    public class Contacto
    {

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "El mensaje es requerido")]
        public string? Mensaje { get; set; }

        public string? Etiqueta { get; set; }
        public float Puntuacion { get; set; }


        [DataType(DataType.DateTime)]
        public DateTime FechaEnvio { get; set; } = DateTime.Now;
    }
}