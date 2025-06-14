using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualBasic;

namespace mercharteria.Models
{
    public class Personaje
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Nombre { get; set; }

        public string? Imagen { get; set; }

        [NotMapped]
        public IFormFile? ImagenFile { get; set; }  

        public string? BannerUrl { get; set; }

        [NotMapped]
        public IFormFile? BannerFile { get; set; }

        public virtual ICollection<Producto>? Productos { get; set; }
        
        public string? SpotifyPlaylistId { get; set; }
    }
}