using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mercharteria.Models;
using mercharteria.Data;


namespace mercharteria.Controllers
{
   
    public class ProductosController : Controller
    {
        private readonly ILogger<ProductosController> _logger;
        private readonly ApplicationDbContext _context;

        public ProductosController(ILogger<ProductosController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {  
            var productos = _context.DbSetProducto.ToList();
            _logger.LogInformation("Productos: {0}", productos);
            return View(productos);
            
        }

         
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}