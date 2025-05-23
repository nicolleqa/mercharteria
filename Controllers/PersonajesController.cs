using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using mercharteria.Models;
using mercharteria.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using mercharteria.Helpers;
using mercharteria.Services;

namespace mercharteria.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class PersonajesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonajesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Personaje personaje)
        {
            if (ModelState.IsValid)
            {
                if (personaje.ImagenFile != null)
                {
                    var helper = new FirebaseStorageHelper();
                    personaje.Imagen = await helper.SubirImagenAutenticadoAsync(personaje.ImagenFile);
                }

                if (personaje.BannerFile != null)
                {
                    var helper = new FirebaseStorageHelper();
                    personaje.BannerUrl = await helper.SubirImagenAutenticadoAsync(personaje.BannerFile);
                }

                _context.Add(personaje);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Personaje creado correctamente.";
                return RedirectToAction("Admin");
            }
            return View(personaje);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Admin()
        {
            var personajes = await _context.Personajes.ToListAsync();
            return View(personajes);
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
                    var helper = new FirebaseStorageHelper();
                    await helper.EliminarImagenAsync(personaje.Imagen);
                }
                if (!string.IsNullOrEmpty(personaje.BannerUrl))
                {
                    var helper = new FirebaseStorageHelper();
                    await helper.EliminarImagenAsync(personaje.BannerUrl);
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
        
        [Authorize(Roles = "Administrador")]
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
                        var helper = new FirebaseStorageHelper();
                        if (!string.IsNullOrEmpty(personaje.Imagen))
                        {
                            await helper.EliminarImagenAsync(personaje.Imagen);
                        }
                        personaje.Imagen = await helper.SubirImagenAutenticadoAsync(personaje.ImagenFile);
                    }

                    if (personaje.BannerFile != null)
                    {
                        var helper = new FirebaseStorageHelper();
                        if (!string.IsNullOrEmpty(personaje.BannerUrl))
                        {
                            await helper.EliminarImagenAsync(personaje.BannerUrl);
                        }
                        personaje.BannerUrl = await helper.SubirImagenAutenticadoAsync(personaje.BannerFile);
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

        private bool PersonajeExists(int id)
        {
            return _context.Personajes.Any(e => e.Id == id);
        }
    }
}