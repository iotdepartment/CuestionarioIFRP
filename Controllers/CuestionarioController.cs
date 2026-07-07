using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace CuestionarioIFRP.Controllers
{
    public class CuestionarioController : Controller
    {
        private readonly global::CuestionarioIFRP.Models.AppDbContext _context;

        public CuestionarioController(global::CuestionarioIFRP.Models.AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // DEJA ÚNICAMENTE ESTE MÉTODO POST (Borra cualquier otra versión de "Guardar")
        [HttpPost]
        public IActionResult Guardar(global::CuestionarioIFRP.Models.CuestionarioIFRP cuestionario)
        {
            var stringBuilder = new StringBuilder();

            var mapeoEstudios = new Dictionary<string, string>
            {
                { "estudios_sinFormacion", "Sin Formación" },
                { "estudios_primaria", "Primaria" },
                { "estudios_secundaria", "Secundaria" },
                { "estudios_bachillerato", "Preparatoria/Bachillerato" },
                { "estudios_tecnico", "Técnico Superior" },
                { "estudios_licenciatura", "Licenciatura" },
                { "estudios_maestria", "Maestría" },
                { "estudios_doctorado", "Doctorado" }
            };

            foreach (var item in mapeoEstudios)
            {
                string respuestaFila = Request.Form[item.Key];

                if (!string.IsNullOrEmpty(respuestaFila))
                {
                    if (stringBuilder.Length > 0) stringBuilder.Append(", ");
                    stringBuilder.Append($"{item.Value}: {respuestaFila}");
                }
            }

            cuestionario.Pregunta4 = stringBuilder.ToString();
            cuestionario.Fecha = DateTime.Today;

            ModelState.Remove("Pregunta4");

            if (ModelState.IsValid)
            {
                _context.CuestionarioIFRP.Add(cuestionario);
                _context.SaveChanges();

                // Variable que detectará la vista para abrir el modal
                ViewBag.MostrarModalExito = true;

                return View("Index", new global::CuestionarioIFRP.Models.CuestionarioIFRP());
            }
            return View("Index", cuestionario);
        }

    }
}
