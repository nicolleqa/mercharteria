using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MercadoPago.Config;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mercharteria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagoApiController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PagoApiController(IConfiguration configuration)
        {
            _configuration = configuration;
            // Inicializar MercadoPago con AccessToken
            MercadoPagoConfig.AccessToken = _configuration["MercadoPago:AccessToken"];
        }

        /// <summary>
        /// Crea una preferencia de pago en MercadoPago y retorna el link de pago.
        /// </summary>
        /// <param name="request">Datos del pago</param>
        /// <returns>URL de pago de MercadoPago</returns>
        [HttpPost]
        public async Task<IActionResult> CrearPago([FromBody] PagoRequest request)
        {
            if (request.Monto <= 0)
                return BadRequest(new { mensaje = "El monto debe ser mayor a cero." });

            var preferenceClient = new PreferenceClient();
            var preferenceRequest = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
                {
                    new PreferenceItemRequest
                    {
                        Title = "Pago de pedido",
                        Quantity = 1,
                        CurrencyId = "PEN",
                        UnitPrice = request.Monto
                    }
                },
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = request.UrlExito ?? "https://localhost:7299/Pago/Confirmacion",
                    Failure = request.UrlFallo ?? "https://localhost:7273/Failure",
                    Pending = request.UrlPendiente ?? "https://localhost:7273/Pending"
                },
                AutoReturn = "approved"
            };

            Preference preference = await preferenceClient.CreateAsync(preferenceRequest);

            return Ok(new
            {
                mensaje = "Preferencia creada correctamente",
                preferenceId = preference.Id,
                initPoint = preference.InitPoint,
                sandboxInitPoint = preference.SandboxInitPoint
            });
        }
        // ...existing code...

/// <summary>
/// Consulta una preferencia de pago de MercadoPago por su ID.
/// </summary>
/// <param name="id">ID de la preferencia</param>
/// <returns>Datos de la preferencia</returns>
[HttpGet("{id}")]
public async Task<IActionResult> ObtenerPago(string id)
{
    var preferenceClient = new PreferenceClient();
    try
    {
        var preference = await preferenceClient.GetAsync(id);
        return Ok(new
        {
            preferenceId = preference.Id,
            // estado = preference.Status,
            items = preference.Items,
            initPoint = preference.InitPoint,
            sandboxInitPoint = preference.SandboxInitPoint
        });
    }
    catch
    {
        return NotFound(new { mensaje = "Preferencia no encontrada." });
    }
}
    }

    /// <summary>
    /// Modelo para solicitar un pago.
    /// </summary>
    public class PagoRequest
    {
        /// <example>100.50</example>
        public decimal Monto { get; set; }
        /// <example>https://localhost:7299/Pago/Confirmacion</example>
        public string UrlExito { get; set; }
        /// <example>https://localhost:7273/Failure</example>
        public string UrlFallo { get; set; }
        /// <example>https://localhost:7273/Pending</example>
        public string UrlPendiente { get; set; }
    }
}