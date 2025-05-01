using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mercharteria.Models;
using mercharteria.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


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

        

    }
}