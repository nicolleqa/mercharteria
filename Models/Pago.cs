using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mercharteria.Models
{
    [Table("t_pago")]
    public class Pago
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        public DateTime FechaPago { get; set; }

        public string? NombreTarjeta { get; set; }

        public string? NumeroTarjeta { get; set; }

        [NotMapped]
        public string? DueDateYYMM { get; set; }

        [NotMapped]
        public string? Cvv { get; set; }

        public decimal MontoTotal { get; set; }

        public string? Estado { get; set; }

        public string? UserName { get; set; }

        // 🔗 NUEVAS PROPIEDADES PARA RELACIÓN CON DATOSCLIENTE

        public int? DatosClienteId { get; set; }  // Clave foránea

        public DatosCliente? DatosCliente { get; set; }  // Propiedad de navegación
    }
}
