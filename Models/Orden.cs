using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace mercharteria.Models
{
    [Table("t_orden")]
    public class Orden
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        public string? UserName { get; set; }

        public decimal Total { get; set; }

        public DateTime Fecha { get; set; }

        // Nuevo: FK a Pago
        public int? PagoId { get; set; }
        [ForeignKey("PagoId")]
        public virtual Pago? Pago { get; set; }

        // Nuevo: FK a DatosCliente
        public int? DatosClienteId { get; set; }
        [ForeignKey("DatosClienteId")]
        public virtual DatosCliente? DatosCliente { get; set; }

        // Nuevo: colección de detalles
        public virtual ICollection<DetalleOrden>? Detalles { get; set; }

        public string? Estado { get; set; }
    }
}
