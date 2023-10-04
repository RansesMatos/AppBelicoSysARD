using BelicoSysApp.Models;
using BelicoSysApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

            // TODO: Get options from API
            // NOTE: When pagination added, filtering should be performed on the backend
            var ddPertrechoOptions = lista.Select(x => x.PertrechosDescripcion).ToList().Distinct();
            var ddAlmacenOptions = lista.Select(x => x.IdAlmacen).ToList().Distinct();

            //lista = lista.Take(50).ToList();

            ViewBag.Pertrecho = lista;

            ViewBag.ddPertrechoOptions = ddPertrechoOptions;
            ViewBag.ddAlmacenOptions = ddAlmacenOptions;

            return View(lista);
        }

        [HttpGet]
        public async Task<IActionResult> PertrechoCreate()
        {
            IEnumerable<Pertrecho> listaA = await _apiServicePertrecho.GetPertrecho();
            var listaPDto = new List<Pertrecho>();

            foreach (var pertrecho in listaA)
            {
                listaPDto.Add(pertrecho);
            }
            ViewBag.DPertrecho = new SelectList(listaPDto, "IdPertrechos", "PertrechosDescripcion");

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
