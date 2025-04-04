using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mercharteria.Models;


namespace mercharteria.Controllers
{
   
    public class ProductosController : Controller
    {
        private readonly ILogger<ProductosController> _logger;

        public ProductosController(ILogger<ProductosController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        { 

            var productos = new List<Producto>{
                new Producto{
                    Id = 1,
                    Nombre = "Camiseta Exclusiva",
                    Descripcion = "Diseño único y cómodo, disponible en varias tallas y colores.",
                    Precio = 25.99m,
                    ImagenUrl = "https://kacaoshop.com/wp-content/uploads/2020/12/Camiseta-Exclusiva-Monchito.jpg"
                },
                new Producto{
                    Id = 2,
                    Nombre = "Taza 3D Spiderman",
                    Descripcion = "Ideal para tus bebidas. Personaliza con tu nombre o imagen favorita.",
                    Precio = 14.99m,
                    ImagenUrl = "https://abejareinaperu.com/wp-content/uploads/2021/06/taza-spiderman-550x550-2.jpeg"
                },
                 new Producto{
                    Id = 3,
                    Nombre = "Gorra Bordada",
                    Descripcion = "Estilo y protección, con bordados de alta calidad.",
                    Precio = 19.99m,
                    ImagenUrl = "https://lacamaleona.com/cdn/shop/files/396971535498460bbff29ba5f40e3de1xl_2048x.jpg?v=1687960992"
                },
                new Producto{
                    Id = 4,
                    Nombre = "Chaqueta estilo Punk de estilo Lobo bordado",
                    Descripcion = "Suave y abrigador, ideal para cualquier temporada.",
                    Precio = 39.99m,
                    ImagenUrl = "https://images.nexusapp.co/assets/9e/6d/ae/26029831.jpg"
                },
                new Producto{
                    Id = 5,
                    Nombre = "Muñecos de Kratos God of War",
                    Descripcion = "Figura coleccionable del famoso personaje de videojuegos.",
                    Precio = 39.99m,
                    ImagenUrl = "https://juguetesdecoleccion.com/wp-content/uploads/figura-kratos-god-of-war-iron-studios.jpg"
                },
                new Producto{
                    Id = 6,
                    Nombre = "Polos para dama modelo crops rock metal",
                    Descripcion = "Estilo rockero para mujeres.",
                    Precio = 29.99m,
                    ImagenUrl = "https://limarockshop.com/wp-content/uploads/2023/09/1000285866.jpg"
                },
                new Producto{
                    Id = 7,
                    Nombre = "Hello Kitty - Case iPhone",
                    Descripcion = "Funda protectora para iPhone con diseño de Hello Kitty.",
                    Precio = 19.99m,
                    ImagenUrl = "https://casemania.com.pe/wp-content/uploads/2024/11/HELLO-KITTY.png"
                },
                new Producto{
                    Id = 8,
                    Nombre = "Estuches para Cargadores",
                    Descripcion = "Protectores prácticos para tus cargadores.",
                    Precio = 12.99m,
                    ImagenUrl = "https://m.media-amazon.com/images/I/61pufWaZJVL._AC_SL1500_.jpg"
                },
                new Producto{
                    Id = 9,
                    Nombre = "Poster de AIRBOURNE en Lima",
                    Descripcion = "Póster conmemorativo del concierto.",
                    Precio = 9.99m,
                    ImagenUrl = "https://buhorockshop.com/wp-content/uploads/2021/11/poster-airbourne-en-lima-450x450.jpg"
                },
                new Producto{
                    Id = 10,
                    Nombre = "Cojín personalizado I Love You",
                    Descripcion = "Cojín con diseño personalizable.",
                    Precio = 22.99m,
                    ImagenUrl = "https://abejareinaperu.com/wp-content/uploads/2022/01/cojin-personalizado-love-you.webp"
                },
                new Producto{
                    Id = 11,
                    Nombre = "Hoodie Premium",
                    Descripcion = "Abrigo cómodo para clima frío.",
                    Precio = 34.99m,
                    ImagenUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRk95YjJ2TVrc0e3bK5M5eY0tdsIlgakSVg1Q&s"
                },
                new Producto{
                    Id = 12,
                    Nombre = "Llavero Hombre Araña",
                    Descripcion = "Llavero metálico de los Avengers.",
                    Precio = 8.99m,
                    ImagenUrl = "https://ohhsorpresa.pe/wp-content/uploads/2019/06/POCKET-POP-0013.jpg"
                },
                new Producto{
                    Id = 13,
                    Nombre = "Mochila Among Us",
                    Descripcion = "Mochila escolar con diseño de videojuego.",
                    Precio = 28.99m,
                    ImagenUrl = "https://calimodpruebaio.vtexassets.com/arquivos/ids/282523/6AMU2020012-1.jpg?v=638713666171370000"
                },
                new Producto{
                    Id = 14,
                    Nombre = "Disfraz Tiburón Inflable",
                    Descripcion = "Divertido disfraz para fiestas.",
                    Precio = 45.99m,
                    ImagenUrl = "https://m.media-amazon.com/images/I/61qmbh1-bIL.jpg"
                },
                new Producto{
                    Id = 15,
                    Nombre = "Pijama Hello Kitty Pareja",
                    Descripcion = "Conjunto de pijama matching.",
                    Precio = 32.99m,
                    ImagenUrl = "https://http2.mlstatic.com/D_774358-MLM81454060538_122024-O.jpg"
                },
                new Producto{
                    Id = 16,
                    Nombre = "Casco Iron Man Electrónico",
                    Descripcion = "Réplica coleccionable con luces.",
                    Precio = 89.99m,
                    ImagenUrl = "https://http2.mlstatic.com/D_NQ_NP_654671-MPE48539260981_122021-O.webp"
                },
                new Producto{
                    Id = 17,
                    Nombre = "Polo 'Se Busca' Huevadas",
                    Descripcion = "Diseño exclusivo de comedia peruana.",
                    Precio = 27.99m,
                    ImagenUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRw5hxyxfd0WNp4HIQsUTw9EzQCQELz_Vi1cw&s"
                },
                new Producto{
                    Id = 18,
                    Nombre = "Pulseras Novia Cadáver",
                    Descripcion = "Dúo de pulseras temáticas.",
                    Precio = 15.99m,
                    ImagenUrl = "https://http2.mlstatic.com/D_NQ_NP_729833-MLM53398165362_012023-O.webp"
                },
                new Producto{
                    Id = 19,
                    Nombre = "Set Harry Potter Escritorio",
                    Descripcion = "Incluye lapicero y marcapáginas.",
                    Precio = 18.99m,
                    ImagenUrl = "https://www.crisol.com.pe/media/catalog/product/cache/f6d2c62455a42b0d712f6c919e880845/8/1/812370014002_k9edbq7vsqokaa4d.jpg"
                },
                new Producto{
                    Id = 20,
                    Nombre = "Collar Shrek Girasol",
                    Descripcion = "Diseño único del ogro favorito.",
                    Precio = 16.99m,
                    ImagenUrl = "https://miho.pe/img/product_color/DivineSherk_girasol.jpeg"
                },
                new Producto{
                    Id = 21,
                    Nombre = "Lámpara 3D Charmander",
                    Descripcion = "Lámpara LED con forma de pokémon.",
                    Precio = 29.99m,
                    ImagenUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSkfUOiND8SyyTZeW9yb5EXn5ISBQk6hV8QQzrQx2VeWkqK2aZNanAce0eO36INoXU8Dt0&usqp=CAU"
                },
                new Producto{
                    Id = 22,
                    Nombre = "Disfraz Chapulín Colorado",
                    Descripcion = "Para niños talla 10.",
                    Precio = 35.99m,
                    ImagenUrl = "https://www.ubuy.pe/productimg/?image=aHR0cHM6Ly9pbWFnZXMtbmEuc3NsLWltYWdlcy1hbWF6b24uY29tL2ltYWdlcy9JLzQxbXYwMDE0MDdMLl9TUzQwMF8uanBn.jpg"
                },
                new Producto{
                    Id = 23,
                    Nombre = "Figura Ellie The Last of Us",
                    Descripcion = "Figura de acción detallada.",
                    Precio = 59.99m,
                    ImagenUrl = "https://inkagames.pe/wp-content/uploads/2023/12/Ellie.jpg"
                },
                new Producto{
                    Id = 24,
                    Nombre = "Gorra Volver al Futuro",
                    Descripcion = "Réplica exacta de la película.",
                    Precio = 24.99m,
                    ImagenUrl = "https://rimage.ripley.com.pe/home.ripley/Attachment/MKP/3578/PMP20000123767/full_image-1.jpeg"
                },
                new Producto{
                    Id = 25,
                    Nombre = "Polera Manga Corta Unisex",
                    Descripcion = "Estilo casual para cualquier ocasión.",
                    Precio = 21.99m,
                    ImagenUrl = "https://cdnx.jumpseller.com/gen-wave1/image/31575695/resize/360/360?1704993082"
                }
                
            };

            return View(productos);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}