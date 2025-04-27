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

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Contacto contacto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contacto);
                _context.SaveChanges();

                TempData["MensajeExito"] = "¡Gracias por contactarnos! Pronto nos comunicaremos contigo.";
                return RedirectToAction(nameof(Index));
            }
            return View(contacto);
        }
    }
}
