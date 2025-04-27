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
                // Obtén los 4 primeros productos de la base de datos
        var productos = _context.DbSetProducto.Take(4).ToList();

        // Pasa los productos a la vista
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
