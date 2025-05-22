using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using mercharteria.Models;
using mercharteria.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Dynamic;
using System.Security.Claims;

namespace mercharteria.Controllers
{
    public class CarritoController : Controller
    {
        private readonly ILogger<CarritoController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;


        public CarritoController(ILogger<CarritoController> logger, 
        UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var userIDSession = _userManager.GetUserName(User);
            if (userIDSession == null)
            {
                ViewData["Message"] = "Por favor debe loguearse antes de agregar un producto";
                return RedirectToAction("Index", "Catalogo");
            }
            var items = from o in _context.DbSetPreOrden select o;
            items = items.Include(p => p.Producto).
                    Where(w => w.UserName.Equals(userIDSession) &&
                        w.Estado.Equals("PENDIENTE"));
            var itemsCarrito = items.ToList();
            var total = itemsCarrito.Sum(c => c.Cantidad * c.Precio);

            dynamic model = new ExpandoObject();
            model.montoTotal = total;
            model.elementosCarrito = itemsCarrito;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int? id, int cantidad)
        {
            var userID = _userManager.GetUserName(User);
            if (userID == null)
            {
                _logger.LogInformation("No existe usuario");
                return Unauthorized(new { message = "Por favor debe loguearse antes de agregar un producto al carrito" });
                // TempData["Message"] = "Por favor debe loguearse antes de agregar un producto";
                // return RedirectToAction("Index", "Productos");
            }

            var producto = await _context.Productos.Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Id == id);
            if (producto == null)
            {
                return NotFound(new { message = "Producto no encontrado" });
                // return NotFound();
            }

            PreOrden proforma = new PreOrden
            {
                Producto = producto,
                Precio = producto.Precio,
                Cantidad = cantidad > 0 ? cantidad : 1,
                UserName = userID
            };
            _context.Add(proforma);
            await _context.SaveChangesAsync();
            // TempData["Message"] = "Se Agrego al carrito";
            _logger.LogInformation("Se agrego un producto al carrito");
            return Ok(new { message = "Producto agregado al carrito correctamente" });
            // return RedirectToAction("Index", "Productos");
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, int cantidad)
        {
            var item = await _context.DbSetPreOrden.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            item.Cantidad = cantidad;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.DbSetPreOrden.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.DbSetPreOrden.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}