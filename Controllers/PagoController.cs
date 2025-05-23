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
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
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
            var pago = new Pago
            {
                UserName = _userManager.GetUserName(User),
                MontoTotal = monto
            };
            _logger.LogInformation("El monto total es: " + pago.MontoTotal.ToString());

            return View(pago);
        }

        [HttpPost]
public IActionResult Pagar(Pago pago)
{
    pago.FechaPago = DateTime.UtcNow;
    pago.Estado = "Cancelado";

    // Buscar el último DatosCliente registrado para este usuario
    var datosCliente = _context.DatosClientes
        .OrderByDescending(d => d.Id)
        .FirstOrDefault();

    pago.DatosCliente = datosCliente;

    _context.Add(pago);
    _context.SaveChanges();


            var itemsCarrito = _context.DbSetPreOrden
                .Include(p => p.Producto)
                .Where(s => s.UserName == pago.UserName && s.Estado == "PENDIENTE")
                .ToList();

            var pedido = new Orden
            {
                UserName = pago.UserName,
                Total = pago.MontoTotal,
                Fecha = DateTime.UtcNow,
                Estado = "Pendiente",
                PagoId = pago.Id
            };
            _context.Add(pedido);
            _context.SaveChanges();

            var detalles = itemsCarrito.Select(item => new DetalleOrden
            {
                OrdenId = pedido.Id,
                ProductoId = item.Producto.Id,
                Precio = item.Precio,
                Cantidad = item.Cantidad,
                Subtotal = item.Precio * item.Cantidad
            }).ToList();
            _context.AddRange(detalles);

            foreach (var p in itemsCarrito)
            {
                p.Estado = "Pagado";
            }
            _context.UpdateRange(itemsCarrito);
            _context.SaveChanges();

            return RedirectToAction("Confirmacion", new { id = pago.Id });
        }

        public IActionResult Confirmacion(int id)
        {
            var pago = _context.DbSetPago.FirstOrDefault(p => p.Id == id);
            if (pago == null)
                return RedirectToAction("Error");

            return View(pago);
        }

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

            TempData.Keep();
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> DatosCliente(DatosCliente datos)
        {
            if (!ModelState.IsValid)
            {
                return View(datos);
            }

            _context.DatosClientes.Add(datos);
            await _context.SaveChangesAsync();

            TempData["NombreCompleto"] = datos.NombreCompleto;
            TempData["Correo"] = datos.Correo;
            TempData["Direccion"] = datos.Direccion;
            TempData["Referencia"] = datos.Referencia;

            return RedirectToAction("Create", new { monto = datos.Monto });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error");
        }
    }
}
