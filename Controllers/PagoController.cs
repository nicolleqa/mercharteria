using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using mercharteria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using mercharteria.Data;

namespace mercharteria.Controllers
{

    public class PagoController : Controller
    {
       
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<PagoController> _logger;
        private readonly ApplicationDbContext _context;

        public PagoController(ILogger<PagoController> logger, 
        UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create(decimal monto)
        {
            Pago pago = new Pago();
            pago.UserName = _userManager.GetUserName(User);
            pago.MontoTotal = monto;
            _logger.LogInformation("El monto total es: " + pago.MontoTotal.ToString());

            return View(pago);
        }

        [HttpPost]
        [HttpPost]
        public IActionResult Pagar(Pago pago)
        {
            // 1. Guardar el pago en la base de datos
            pago.FechaPago = DateTime.UtcNow;
            pago.Estado = "Cancelado"; // Estado del pago
            _context.Add(pago);
            _context.SaveChanges(); // Aquí se genera el ID del pago

            // 2. Obtener los productos del carrito correctamente
            var itemsCarrito = _context.DbSetPreOrden
                .Include(p => p.Producto)
                .Where(s =>
                    s.UserName == pago.UserName &&
                    s.Estado == "PENDIENTE" // 
                )
                .ToList();

            // 3. Crear la orden con la fecha y vincular el pago
            var pedido = new Orden
            {
                UserName = pago.UserName,
                Total = pago.MontoTotal,
                Fecha = DateTime.UtcNow, // Se guarda la fecha actual
                Estado = "Pendiente",
                PagoId = pago.Id
            };
            _context.Add(pedido);
            _context.SaveChanges(); // Genera pedido.Id

            // 4. Crear detalles de la orden
            var detalles = itemsCarrito.Select(item => new DetalleOrden
            {
                OrdenId = pedido.Id,
                ProductoId = item.Producto.Id,
                Precio = item.Precio,
                Cantidad = item.Cantidad,
                Subtotal = item.Precio * item.Cantidad
            }).ToList();
            _context.AddRange(detalles);

            // 5. Cambiar estado del carrito a "Pagado"
            foreach (var p in itemsCarrito)
            {
                p.Estado = "Pagado";
            }
            _context.UpdateRange(itemsCarrito);

            // 6. Guardar todo lo pendiente
            _context.SaveChanges();

            ViewData["Mensaje"] = "El pago se ha realizado con éxito¡";
            return View("Create");
        }


        // Acción para mostrar el formulario de datos del cliente
        public IActionResult DatosCliente(decimal monto)
        {
            var modelo = new DatosCliente 
            { 
                Monto = monto,
                NombreCompleto = TempData["NombreCompleto"]?.ToString(),
                Correo = TempData["Correo"]?.ToString(),
                Direccion = TempData["Direccion"]?.ToString(),
                Referencia = TempData["Referencia"]?.ToString()
            };

            TempData.Keep(); // Mantener los datos en TempData para la siguiente carga
            return View(modelo);
        }

        // Acción POST para recibir y guardar los datos del cliente
        [HttpPost]
        public async Task<IActionResult> DatosCliente(DatosCliente datos)
        {
            if (!ModelState.IsValid)
            {
                return View(datos);
            }

            // Guardamos los datos en la base de datos
            _context.DatosClientes.Add(datos);
            await _context.SaveChangesAsync(); // Guardar cambios en la base de datos

            // Almacenamos datos en TempData para mantenerlos entre redirecciones
            TempData["NombreCompleto"] = datos.NombreCompleto;
            TempData["Correo"] = datos.Correo;
            TempData["Direccion"] = datos.Direccion;
            TempData["Referencia"] = datos.Referencia;

            // Redirigimos a la acción Create (o cualquier acción que quieras)
            return RedirectToAction("Create", new { monto = datos.Monto });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}