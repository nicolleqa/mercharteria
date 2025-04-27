using Microsoft.AspNetCore.Mvc;
using mercharteria.Data;
using mercharteria.Models;
using System.Linq;
using System.Diagnostics;

namespace mercharteria.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var personajes = _context.Personajes.ToList();
        ViewBag.Personajes = personajes;

        var testimonios = _context.Testimonios.Take(3).ToList();
        ViewBag.Testimonios = testimonios;

        var productos = _context.DbSetProducto.Take(4).ToList();

        return View(productos);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
