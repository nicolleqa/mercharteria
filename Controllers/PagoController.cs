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
using Microsoft.Extensions.Configuration;
using MercadoPago.Config;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;


namespace mercharteria.Controllers
{
    public class PagoController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<PagoController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public PagoController(
            ILogger<PagoController> logger,
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context,
            IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _configuration = configuration;

            // Inicializar MercadoPago con AccessToken
            MercadoPagoConfig.AccessToken = _configuration["MercadoPago:AccessToken"];
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create(decimal monto)
        {
            var userName = _userManager.GetUserName(User);
            var pago = new Pago
            {
                UserName = userName,
                MontoTotal = monto
            };

            _logger.LogInformation("El monto total es: " + pago.MontoTotal.ToString());

            // Crear preferencia en MercadoPago
            var preferenceClient = new PreferenceClient();
            var request = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
                {
                    new PreferenceItemRequest
                    {
                        Title = "Pago de pedido",
                        Quantity = 1,
                        CurrencyId = "PEN",
                        UnitPrice = monto
                    }
                },
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = "https://localhost:5000/Pago/Confirmacion",
                    Failure = "https://localhost:7273/Failure",
                    Pending = "https://localhost:7273/Pending"
                },

                AutoReturn = "approved"
            };

            Preference preference = await preferenceClient.CreateAsync(request);
            ViewBag.PreferenceId = preference.Id;
            ViewBag.PublicKey = _configuration["MercadoPago:PublicKey"];
            return Redirect(preference.SandboxInitPoint);
            //return View(pago);
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

        // [HttpPost]
        // public async Task<IActionResult> DatosCliente(DatosCliente datos)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return View(datos);
        //     }

        //     _context.DatosClientes.Add(datos);
        //     await _context.SaveChangesAsync();

        //     TempData["NombreCompleto"] = datos.NombreCompleto;
        //     TempData["Correo"] = datos.Correo;
        //     TempData["Direccion"] = datos.Direccion;
        //     TempData["Referencia"] = datos.Referencia;

        //     return RedirectToAction("Create", new { monto = datos.Monto });
        // }

        public IActionResult Confirmacion()
        {
            // Aquí podrías buscar y mostrar el pago por ID si es necesario
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error");
        }

        private string obtenerAccessToken()
        {
            return _configuration["MercadoPago:AccessToken"] ?? string.Empty;
        }

        private string obtenerPublicKey()
        {
            return _configuration["MercadoPago:PublicKey"] ?? string.Empty;
        }
    }
}
