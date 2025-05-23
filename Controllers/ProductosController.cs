using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using mercharteria.Models;
using mercharteria.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using mercharteria.Helpers;
using mercharteria.Services;


namespace mercharteria.Controllers
{
   
    public class ProductosController : Controller
    {
        private readonly ILogger<ProductosController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly FirebaseStorageHelper _firebaseStorageHelper;

        private readonly SpotifyService _spotifyService;

        public ProductosController(ILogger<ProductosController> logger, ApplicationDbContext context, FirebaseStorageHelper firebaseStorageHelper, SpotifyService spotifyService)
        {
            _firebaseStorageHelper = firebaseStorageHelper;
            _logger = logger;
            _context = context;
            _spotifyService = spotifyService;
        }

        public IActionResult Index(int? categoriaId)
        {  
            var query = _context.Productos.Include(p => p.Categoria).AsQueryable();
            if (categoriaId.HasValue && categoriaId.Value != 0)
            {
                query = query.Where(p => p.CategoriaId == categoriaId);
            }

            ViewBag.Categorias = _context.Categorias.ToList();
            return View(query.ToList());
        }
        public async Task<IActionResult> Details(int? id)
        {
            Producto? objProduct = await _context.Productos.FindAsync(id);
            if (objProduct == null)
            {
                return NotFound();
            }
            return View(objProduct);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
        

        [Authorize(Roles = "Administrador")]
        public IActionResult Admin()
        {
            var productos = _context.Productos
                .Include(p => p.Categoria)
                .ToList();
            return View(productos);
        }

        
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            ViewBag.CategoriaId = new SelectList(_context.Categorias, "Id", "Nombre");
            ViewBag.Personajes = new MultiSelectList(_context.Personajes, "Id", "Nombre");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(Producto producto, int[] PersonajeIds)
        {
            if (ModelState.IsValid)
            {
                if (producto.ImagenFile != null)
                {
                    
                    var url = await _firebaseStorageHelper.SubirImagenAutenticadoAsync(producto.ImagenFile);
                    producto.ImagenUrl = url;
                }

                if (PersonajeIds != null && PersonajeIds.Any())
                {
                    producto.Personajes = await _context.Personajes
                        .Where(p => PersonajeIds.Contains(p.Id))
                        .ToListAsync();
                }

                _context.Add(producto);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Producto creado correctamente.";
                return RedirectToAction(nameof(Admin));

            }
            ViewBag.CategoriaId = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);
            ViewBag.Personajes = new MultiSelectList(_context.Personajes, "Id", "Nombre");
            return View(producto);
        }


        

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id)
        {
           var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            ViewBag.CategoriaId = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id, Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    // Obtener el producto original desde la base de datos
                    var productoExistente = await _context.Productos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                    if (productoExistente == null)
                    {
                        return NotFound();
                    }

                   

                    // Si se subió una nueva imagen
                    if (producto.ImagenFile != null)
                    {
                        //Eliminar imagen anterior de Firebase si existe
                        if (!string.IsNullOrEmpty(productoExistente.ImagenUrl))
                        {
                            await _firebaseStorageHelper.EliminarImagenAsync(productoExistente.ImagenUrl);
                        }

                        //Subir la nueva imagen y asignar URL
                        var nuevaUrl = await _firebaseStorageHelper.SubirImagenAutenticadoAsync(producto.ImagenFile);
                        producto.ImagenUrl = nuevaUrl;
                        
                    }
                    else
                    {
                        // Si no se subió imagen nueva, mantenemos la URL anterior
                        producto.ImagenUrl = productoExistente.ImagenUrl;
                    }

                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Producto editado correctamente.";
                    return RedirectToAction(nameof(Admin));
                }

                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Ocurrió un error al actualizar el producto. Detalles: " + ex.Message);
                }
            }

            ViewBag.CategoriaId = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);
            return View(producto);
        }



        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            try
            {
                // Eliminar la imagen de Firebase
                if (!string.IsNullOrEmpty(producto.ImagenUrl))
                {
                   
                    await _firebaseStorageHelper.EliminarImagenAsync(producto.ImagenUrl);
                }

                // Eliminar el producto de la base de datos
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Producto eliminado correctamente.";
            }
            catch (Exception)
            {
                TempData["Message"] = "No se puede eliminar el producto.";
                return RedirectToAction(nameof(Admin));
            }

            return RedirectToAction(nameof(Admin));
        }

        public async Task<IActionResult> PorPersonaje(int id)
        {
            try
            {
                var personaje = await _context.Personajes
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (personaje == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var productos = await _context.Productos
                    .Include(p => p.Categoria)
                    .Include(p => p.Personajes)
                    .Where(p => p.Personajes.Any(per => per.Id == id))
                    .ToListAsync();

                ViewBag.PersonajeNombre = personaje.Nombre;
                ViewBag.PersonajeBanner = personaje.BannerUrl;
                ViewBag.Categorias = await _context.Categorias.ToListAsync();

                if (!string.IsNullOrEmpty(personaje.SpotifyPlaylistId))
                {
                    ViewBag.SpotifyPlaylistUrl = await _spotifyService.GetPlaylistEmbedUrl(personaje.SpotifyPlaylistId);
                }

                return View(productos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos por personaje");
                return RedirectToAction("Index", "Home");
            }
        }


        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }


    }
}