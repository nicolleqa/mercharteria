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

        public string? Estado { get; set; }

        // Relación con Pago (que a su vez contiene DatosCliente)
        public int? PagoId { get; set; }
        [ForeignKey("PagoId")]
        public virtual Pago? Pago { get; set; }

        // Los datos del cliente se obtienen desde el Pago      
        [NotMapped]
        public DatosCliente? DatosCliente
        {
            get => Pago?.DatosCliente;
            set { /* necesario para evitar error de EF Core */ }
        }



        // Detalles de la orden
        public virtual ICollection<DetalleOrden> Detalles { get; set; } = new List<DetalleOrden>();

        // Propiedad para saber si está pagada
        [NotMapped]
        public bool EstaPagado => Pago != null;
    }
}
