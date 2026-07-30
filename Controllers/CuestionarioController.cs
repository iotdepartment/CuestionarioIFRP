using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace CuestionarioIFRP.Controllers
{
    public class CuestionarioController : Controller
    {
        private readonly global::CuestionarioIFRP.Models.AppDbContext _context;      // Conectado a la BD 'RH'
        private readonly global::CuestionarioIFRP.Models.UserContext _userContext;  // Conectado a la BD 'TGRMX'

        public CuestionarioController(
            global::CuestionarioIFRP.Models.AppDbContext context,
            global::CuestionarioIFRP.Models.UserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

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
                // CORREGIDO: Guarda usando _context (Aprieta el gatillo directo en la BD 'RH')
                _context.CuestionarioIFRP.Add(cuestionario);
                _context.SaveChanges();

                ViewBag.MostrarModalExito = true;

                return View("Index", new global::CuestionarioIFRP.Models.CuestionarioIFRP());
            }
            return View("Index", cuestionario);
        }

        [HttpGet]
        public IActionResult BuscarEmpleado(string filtro)
        {
            if (string.IsNullOrEmpty(filtro))
            {
                return Json(new List<object>());
            }

            filtro = filtro.Trim().ToLower();

            try
            {
                List<global::CuestionarioIFRP.Models.rh4> listaEmpleados = new List<global::CuestionarioIFRP.Models.rh4>();

                // CORREGIDO: Usa _userContext para realizar una consulta LINQ local y directa en la BD 'TGRMX'
                if (int.TryParse(filtro, out int numeroEmpleado))
                {
                    listaEmpleados = _userContext.rh4
                        .Where(e => e.EMPLEADO == numeroEmpleado)
                        .Take(5)
                        .ToList();
                }
                else
                {
                    listaEmpleados = _userContext.rh4
                        .Where(e => e.NOMBRECOMPLETO != null && e.NOMBRECOMPLETO.ToLower().Contains(filtro))
                        .Take(5)
                        .ToList();
                }

                var resultado = listaEmpleados.Select(e => new
                {
                    numero = e.EMPLEADO,
                    nombre = e.NOMBRECOMPLETO,
                    puesto = e.PUESTO_DESCRIPCION
                }).ToList();

                return Json(resultado);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en consulta nativa de TGRMX: " + ex.Message);
                return StatusCode(500, "Error de comunicación con la base de datos de empleados.");
            }
        }
    }
}
