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
        public IActionResult Pagar(Pago pago)
        {
            // 1. Guardar el pago primero en la base de datos
            pago.FechaPago = DateTime.UtcNow;
            pago.Estado = "Cancelado"; // o como desees
            _context.Add(pago);
            _context.SaveChanges(); // Aquí se genera el ID

            // 2. Obtener los productos del carrito
            var itemsCarrito = _context.DbSetPreOrden
            .Include(p => p.Producto)
            .Where(s => s.UserName.Equals(pago.UserName) && s.Estado.Equals("Pendiente"));

            // 3. Crear la orden y asignar el pago ya con ID
            Orden pedido = new Orden
            {
                UserName = pago.UserName,
                Total = pago.MontoTotal,
                Pago = pago,
                Estado = "Pendiente"
            };
                _context.Add(pedido);

            // 4. Agregar los detalles de la orden
            List<DetalleOrden> itemsPedido = new List<DetalleOrden>();
            foreach (var item in itemsCarrito.ToList())
            {
                DetalleOrden detallePedido = new DetalleOrden
            {
                Orden = pedido,
                Precio = item.Precio,
                Producto = item.Producto,
                Cantidad = item.Cantidad,
                Subtotal = item.Precio * item.Cantidad,
                OrdenId = pedido.Id,
                ProductoId = item.Producto.Id
            };
                itemsPedido.Add(detallePedido);
            }

                _context.AddRange(itemsPedido);

            // 5. Actualizar estado del carrito
            foreach (PreOrden p in itemsCarrito.ToList())
            {
                p.Estado = "Pagado";
            }
                _context.UpdateRange(itemsCarrito);

            // 6. Guardar todo lo restante
                _context.SaveChanges();

                ViewData["Mensaje"] = "El pago se ha realizado con éxito¡";

                return View("Create");
        }

        // Acción para mostrar el formulario de datos del cliente
        public IActionResult DatosCliente(decimal monto)
        {
            var modelo = new DatosCliente { Monto = monto };
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