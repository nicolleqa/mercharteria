using System.ComponentModel.DataAnnotations;

namespace mercharteria.Models
{
    public class ProductoPersonaje
    {
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        public int PersonajeId { get; set; }
        public Personaje? Personaje { get; set; }
    }
}