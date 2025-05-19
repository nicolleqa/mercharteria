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


namespace mercharteria.Controllers
{
   
    public class ProductosController : Controller
    {
        private readonly ILogger<ProductosController> _logger;
        private readonly ApplicationDbContext _context;

        public ProductosController(ILogger<ProductosController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
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
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                if (producto.ImagenFile != null)
                {
                    var helper = new FirebaseStorageHelper();
                    var url = await helper.SubirImagenAutenticadoAsync(producto.ImagenFile);
                    producto.ImagenUrl = url;
                }

                _context.Add(producto);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Producto creado correctamente.";
                return RedirectToAction(nameof(Admin));

            }
            ViewBag.CategoriaId = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);
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
                if (producto.ImagenFile != null)
                {
                    var helper = new FirebaseStorageHelper();
                    var url = await helper.SubirImagenAutenticadoAsync(producto.ImagenFile);
                    producto.ImagenUrl = url;
                }

                _context.Add(producto);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Producto editado correctamente.";
                return RedirectToAction(nameof(Admin));
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
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Producto eliminado correctamente.";
            }
            catch (Exception)
            {
                TempData["Error"] = "No se puede eliminar el producto.";
                return RedirectToAction(nameof(Admin));
            }

            return RedirectToAction(nameof(Admin));
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }


    }
}