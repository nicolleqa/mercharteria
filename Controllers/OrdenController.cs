using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mercharteria.Data;

namespace mercharteria.Controllers
{
    [Authorize]
    public class OrdenController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdenController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Orden/Historial
        public async Task<IActionResult> Historial()
        {
            var userName = _userManager.GetUserName(User);

            var ordenes = await _context.DbSetOrden
                .Where(o => o.UserName == userName)
                .Include(o => o.Pago)
                .Include(o => o.DatosCliente)
                .Include(o => o.Detalles!)
                    .ThenInclude(d => d.Producto)
                .OrderByDescending(o => o.Fecha)
                .ToListAsync();

            return View(ordenes);
        }
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Admin()
        {
            var ordenes = await _context.DbSetOrden
                .Include(o => o.Detalles!)
                    .ThenInclude(d => d.Producto)
                .OrderByDescending(o => o.Fecha)
                .ToListAsync();

            return View(ordenes);
        }
        [Authorize(Roles = "Administrador")]
            [Authorize(Roles = "Administrador")]
    [Authorize(Roles = "Administrador")]
public async Task<IActionResult> Detalle(int id)
{
    var orden = await _context.DbSetOrden
        .Include(o => o.Pago)
            .ThenInclude(p => p.DatosCliente)
        .Include(o => o.Detalles!)
            .ThenInclude(d => d.Producto)
        .FirstOrDefaultAsync(o => o.Id == id);

    if (orden == null)
    {
        return NotFound();
    }

    return View(orden);
}


    }
}
