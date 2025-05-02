using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace mercharteria.Models
{
    [Table("t_DatosCliente")]
    public class DatosCliente
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        [Display(Name = "Nombre Completo")]
        public string? NombreCompleto { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        public string? Correo { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string? Direccion { get; set; }

        [Display(Name = "Referencia (opcional)")]
        public string? Referencia { get; set; }

        public decimal Monto { get; set; }
    }
}