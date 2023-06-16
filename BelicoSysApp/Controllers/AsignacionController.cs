using BelicoSysApp.Models;
using BelicoSysApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BelicoSysApp.Controllers
{
    public class AsignacionController : Controller
    {

            private readonly IApiServiceAsignacion _apiServiceAsignacion;
        public AsignacionController(IApiServiceAsignacion apiServiceAsignacion)
            {
                _apiServiceAsignacion = apiServiceAsignacion;
            }
            public async Task<IActionResult> AsignacionReport()
            {
                ICollection<AsignacionArma> lista = await _apiServiceAsignacion.GetAsignaciones();
                ViewBag.Arma = lista;
                return View(lista);
            }


            [HttpGet]
            public async Task<IActionResult> AsignacionCreate()
            {
                IEnumerable<VArma> lista = await _apiServiceAsignacion.GetVArmas();
                IEnumerable<AsignarEstado> listaAsigEst = await _apiServiceAsignacion.GetAsigEstado();
                var listaDto = new List<VArma>();
                var listaEstDto = new List<AsignarEstado>();
            foreach(var arma in lista)
            {
                listaDto.Add(arma);              
            }
            foreach (var asignarEstado in listaAsigEst)
            {
                listaEstDto.Add(asignarEstado);
            }

            ViewBag.IdArma = new SelectList(listaDto, "IdArma", "ArmaSerie");
            ViewBag.AsignacionTipo = new SelectList(listaEstDto, "IdAsignacionEstado", "AsignacionEstadoDescripcion");


            return View();
            }
            [HttpPost]
            public async Task<IActionResult> AsignacionCreate(AsignacionArma model)
            {

                if (model.IdAsignacion == 0)
                {
                    var respuesta = await _apiServiceAsignacion.Save(model);
                    if (respuesta.IdAsignacion == 0)
                    {
                        ModelState.AddModelError("", "Error el Numero de Serie ya esta registrado");
                    }
                    TempData["SuccessMessage"] = $"Registro Creado Con el ID {respuesta.IdAsignacion}";
                }
                else
                {
                    ModelState.AddModelError("", "Error Contacatar el Administrador");
                }

                return View("MenuAsignacion");
            }



            public IActionResult MenuAsignacion()

            {
                string successMessage = TempData["SuccessMessage"] as string;

                // Pass the success message to the view
                ViewBag.SuccessMessage = successMessage;
                return View();
            }
    }
}


