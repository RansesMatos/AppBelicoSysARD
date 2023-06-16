using BelicoSysApp.Models;
using BelicoSysApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BelicoSysApp.Controllers
{
    public class ArmaController : Controller
    {
        private readonly IApiServiceArma _apiArma;
        public ArmaController(IApiServiceArma apiArma)
        {
            _apiArma = apiArma;
        }
        public async Task<IActionResult> ArmaReport()
        {
            ICollection<VArma> lista = await _apiArma.GetVArmas();
            ViewBag.Arma = lista;
            return View(lista);
        }


        [HttpGet]
        public IActionResult ArmaCreate()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ArmaCreate(Arma model)
        {

            if (model.IdArma == 0)
            {
                var respuesta = await _apiArma.Save(model);
                if (respuesta.IdArma == 0)
                {
                    ModelState.AddModelError("", "Error el Numero de Serie ya esta registrado");
                }
                TempData["SuccessMessage"] = $"Registro Creado Con el ID {respuesta.IdArma}";
            }
            else
            {
                ModelState.AddModelError("", "Error Contacatar el Administrador");
            }

            return View("MenuArma");
        }



        public IActionResult MenuArma()

        {
            string successMessage = TempData["SuccessMessage"] as string;

            // Pass the success message to the view
            ViewBag.SuccessMessage = successMessage;
            return View();
        }



    }

}
