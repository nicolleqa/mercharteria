using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;


namespace mercharteria.Models
{
    [Table("t_producto")]
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [NotNull]
        public string? Nombre { get; set; }
        [NotNull]
        public string? Descripcion { get; set; }
        [NotNull]
        public decimal Precio { get; set; }
        [NotNull]
        public string? ImagenUrl { get; set; }
        
    }
}