using BelicoSysApp.Models;
using BelicoSysApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BelicoSysApp.Controllers
{
    public class PertrechoController : Controller
    {
        private readonly IApiServicePertrecho _apiServicePertrecho;
        public PertrechoController(IApiServicePertrecho apiPertrecho)
        {
            _apiServicePertrecho = apiPertrecho;
        }

        public async Task<IActionResult> PertrechoReport()
        {
            ICollection<Pertrecho> lista = await _apiServicePertrecho.GetPertrecho();
            ViewBag.Pertrecho = lista;
            return View(lista);
        }

        [HttpGet]
        public IActionResult PertrechoCreate()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PertrechoCreate(Pertrecho model)
        {

            if (model.IdPertrechos == 0)
            {
                var respuesta = await _apiServicePertrecho.Save(model);
                if (respuesta.IdPertrechos == 0)
                {
                    ModelState.AddModelError("", "Error el Numero de Serie ya esta registrado");
                }
                TempData["SuccessMessage"] = $"Registro Creado Con el ID {respuesta.IdPertrechos}";
            }
            else
            {
                ModelState.AddModelError("", "Error Contacatar el Administrador");
            }

            return View("MenuPertrecho");
        }



        public IActionResult MenuPertrecho()

        {
            string successMessage = TempData["SuccessMessage"] as string;

            // Pass the success message to the view
            ViewBag.SuccessMessage = successMessage;
            return View();
        }
    }
}
