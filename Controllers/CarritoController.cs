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

        public async Task<IActionResult> Add(int? id)
        {
            var userID = _userManager.GetUserName(User);
            if (userID == null)
            {
                _logger.LogInformation("No existe usuario");
                ViewData["Message"] = "Por favor debe loguearse antes de agregar un producto";
                return RedirectToAction("Index", "Productos");
            }
            else
            {
                var producto = await _context.DbSetProducto.FindAsync(id);
                PreOrden proforma = new PreOrden();
                proforma.Producto = producto;
                proforma.Precio = producto.Precio;
                proforma.Cantidad = 1;
                proforma.UserName = userID;
                _context.Add(proforma);
                await _context.SaveChangesAsync();
                ViewData["Message"] = "Se Agrego al carrito";
                _logger.LogInformation("Se agrego un producto al carrito");
                return RedirectToAction("Index", "Productos");
            }
        }







        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}