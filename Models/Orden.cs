using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace mercharteria.Models
{
   [Table("t_orden")]
    public class Orden
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        public string? UserName { get; set; }

        public Decimal Total { get; set; }

        public DateTime Fecha { get; set; }

        public Pago? Pago { get; set; }


        public string? Estado { get; set; }
    }
}