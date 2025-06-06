using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mercharteria.Models;
using mercharteria.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using mercharteria.ML;

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
                
                // Aquí puedes realizar el análisis de sentimiento del mensaje
                var sampleData = new MLModelSentimentalAnalysis.ModelInput()
                {
                    Comentario = contacto.Mensaje,
                };

                //Load model and predict output
                var result = MLModelSentimentalAnalysis.Predict(sampleData);
                var predictedLabel = result.PredictedLabel;
                var scorePositive = result.Score[0];
                var scoreNegative = result.Score[1];
                //Check if the result is positive or negative
                if (predictedLabel == "1")
                {
                    contacto.Etiqueta = "Positivo";
                    contacto.Puntuacion = scorePositive;
                }
                else
                {
                    contacto.Etiqueta = "Negativo";
                    contacto.Puntuacion = scoreNegative;
                }

                contacto.FechaEnvio = DateTime.UtcNow; 
                _context.Contactos.Add(contacto);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Mensaje enviado correctamente";
                return RedirectToAction(nameof(Index));
            }
            return View("Index", contacto);
        }




        [Authorize(Roles = "Administrador")]
        public IActionResult Admin()
        {
            var contactos = _context.Contactos
                .ToList();
            return View(contactos);
        }
    }
}