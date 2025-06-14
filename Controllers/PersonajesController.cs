using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using mercharteria.Models;
using mercharteria.Data;
using mercharteria.Helpers;

namespace mercharteria.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class PersonajesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly FirebaseStorageHelper _firebaseStorageHelper;

        public PersonajesController(
            ApplicationDbContext context,
            FirebaseStorageHelper firebaseStorageHelper)
        {
            _context = context;
            _firebaseStorageHelper = firebaseStorageHelper;
        }

        public async Task<IActionResult> Admin()
        {
            var personajes = await _context.Personajes.ToListAsync();
            return View(personajes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Personaje personaje)
        {
            if (ModelState.IsValid)
            {
                if (personaje.ImagenFile != null)
                {
                    personaje.Imagen = await _firebaseStorageHelper.SubirImagenAutenticadoAsync(personaje.ImagenFile);
                }

                if (personaje.BannerFile != null)
                {
                    personaje.BannerUrl = await _firebaseStorageHelper.SubirImagenAutenticadoAsync(personaje.BannerFile);
                }

                _context.Add(personaje);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Personaje creado correctamente.";
                return RedirectToAction(nameof(Admin));
            }
            return View(personaje);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var personaje = await _context.Personajes.FindAsync(id);
            if (personaje == null)
            {
                return NotFound();
            }
            return View(personaje);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Personaje personaje)
        {
            if (id != personaje.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (personaje.ImagenFile != null)
                    {
                        if (!string.IsNullOrEmpty(personaje.Imagen))
                        {
                            await _firebaseStorageHelper.EliminarImagenAsync(personaje.Imagen);
                        }
                        personaje.Imagen = await _firebaseStorageHelper.SubirImagenAutenticadoAsync(personaje.ImagenFile);
                    }

                    if (personaje.BannerFile != null)
                    {
                        if (!string.IsNullOrEmpty(personaje.BannerUrl))
                        {
                            await _firebaseStorageHelper.EliminarImagenAsync(personaje.BannerUrl);
                        }
                        personaje.BannerUrl = await _firebaseStorageHelper.SubirImagenAutenticadoAsync(personaje.BannerFile);
                    }

                    _context.Update(personaje);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Personaje actualizado correctamente.";
                    return RedirectToAction(nameof(Admin));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonajeExists(personaje.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(personaje);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var personaje = await _context.Personajes.FindAsync(id);
            if (personaje == null)
            {
                return NotFound();
            }

            try
            {
                if (!string.IsNullOrEmpty(personaje.Imagen))
                {
                    await _firebaseStorageHelper.EliminarImagenAsync(personaje.Imagen);
                }
                if (!string.IsNullOrEmpty(personaje.BannerUrl))
                {
                    await _firebaseStorageHelper.EliminarImagenAsync(personaje.BannerUrl);
                }

                _context.Personajes.Remove(personaje);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Personaje eliminado correctamente.";
            }
            catch (Exception)
            {
                TempData["Error"] = "No se puede eliminar el personaje.";
            }

            return RedirectToAction(nameof(Admin));
        }

        private bool PersonajeExists(int id)
        {
            return _context.Personajes.Any(e => e.Id == id);
        }
    }
}