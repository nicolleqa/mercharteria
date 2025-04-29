using Microsoft.AspNetCore.Mvc;
using mercharteria.Models;
using mercharteria.Data;

namespace mercharteria.Controllers
{
    public class ContactoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EnviarMensaje(Contacto contacto)
        {
            if (ModelState.IsValid)
            {
                contacto.FechaEnvio = DateTime.UtcNow; 
                _context.Contactos.Add(contacto);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Mensaje enviado correctamente";
                return RedirectToAction(nameof(Index));
            }
            return View("Index", contacto);
        }
    }
}