using BelicoSysApp.Models;
using BelicoSysApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

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

            // TODO: Get options from API
            // NOTE: When pagination added, filtering should be performed on the backend
            var ddRangoOptions = lista.Select(x => x.Asignacion_rango).ToList().Distinct();

            //lista = lista.Take(30).ToList();

            ViewBag.Arma = lista;
            ViewBag.ddRangoOptions = ddRangoOptions;

            return View(lista);
        }

        [HttpGet]
        public async Task<IActionResult> SearchPeople(string nombre)
        {
            string status = "A";
            IEnumerable<VPersonal> lista = await _apiServiceAsignacion.GetVPersonal(nombre, status);
            var listaDto = new List<VPersonal>();
            
            foreach (var personal in lista)
            {
                listaDto.Add(personal);
            }

             var data = new SelectList(listaDto, "MilitarNo", "Nombres");
            ViewBag.Pertrecho = new SelectList(listaDto, "MilitarNo", "Nombres");


            return Ok(data);
        }
       
        [HttpGet]
        public async Task<IActionResult> SearchPeopleDoc(string nombre)
        {
            string status = "A";
            IEnumerable<VPersonal> lista = await _apiServiceAsignacion.GetVPersonal(nombre, status);
            var listaDto = new List<VPersonal>();

            foreach (var personal in lista)
            {
                listaDto.Add(personal);
            }

            var data = new SelectList(listaDto, "MilitarNo", "Doc");
            ViewBag.Personal = new SelectList(listaDto, "MilitarNo", "Nombres");


            return Ok(data);
        }
        [HttpGet]
        public IActionResult DescargoArma()
        {

            ViewBag.count = 0;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DescargoArma(VPersonal codigo)
        {
            var obtasig = codigo.MilitarNo;
            IEnumerable<AsignacionArma> lisAsig =  _apiServiceAsignacion.GetAsignaciones().Result.Where(e => e.AsignacionNoRango == obtasig);
            IEnumerable<VArma> lista = await _apiServiceAsignacion.GetVArmas();
            var listaDto = new List<VArma>();
            var listaasigDto = new List<AsignacionArma>();
            int? valorarma = 0;
           
                foreach (var asig in lisAsig)
                {
                    listaasigDto.Add(asig);
                    valorarma = asig.IdArma;
                    foreach (var arma in lista.Where(e => e.IdArma == valorarma))
                    {
                    listaDto.Add(arma);
                    }
                }
                

                //foreach (var arma in lista.Where(e => e.IdArma == nuevoC))
                //{
                //    listaDto.Add(arma);
                //}
                ViewBag.count = listaasigDto.Count;
                ViewBag.Arma = listaDto;

                VPersonal listaP = await _apiServiceAsignacion.GetVPersonaId(codigo.MilitarNo);

                ViewBag.Nombres = listaP.Nombres;
                ViewBag.desc_rango = listaP.desc_rango;
                ViewBag.Cedula = listaP.Cedula;
            

            


            return View();
        }


        [HttpGet]
        public async Task<IActionResult> AsignacionCreate()
            {
            IEnumerable<Pertrecho> listaP = await _apiServiceAsignacion.Getpertrechos();
            IEnumerable<VArma> lista = await _apiServiceAsignacion.GetVArmas();
                IEnumerable<AsignarEstado> listaAsigEst = await _apiServiceAsignacion.GetAsigEstado();
                var listaDto = new List<VArma>();
                var listaEstDto = new List<AsignarEstado>();
            var listaDtop = new List<Pertrecho>();
            foreach (var pertrecho in listaP)
            {
                listaDtop.Add(pertrecho);
            }

            var data = new SelectList(listaDto);
            // ViewBag.pertrecho = new SelectList(listaDto, "PertrechosDescripcion", "Cantidad");
            foreach (var arma in lista)
            {
                listaDto.Add(arma);              
            }
            foreach (var asignarEstado in listaAsigEst)
            {
                listaEstDto.Add(asignarEstado);
            }
            
            ViewBag.IdArma = new SelectList(listaDto, "IdArma", "ArmaSerie");
            ViewBag.pertrecho = new SelectList(listaDtop, "PertrechosDescripcion", "PertrechosDescripcion");
            ViewBag.AsignacionEstado = new SelectList(listaEstDto, "IdAsignacionEstado", "AsignacionEstadoDescripcion");
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

        [HttpGet]
        public async Task<IActionResult> PeronaId(decimal id) {
            var itemPersona =  await _apiServiceAsignacion.GetVPersonaId(id);

            if (itemPersona != null) {

                var itemPDto = new ApiResult();

                itemPDto.vPersonal = itemPersona;

                return Ok(itemPDto.vPersonal.Cedula);
            }
            
           

            



            return NotFound();
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


