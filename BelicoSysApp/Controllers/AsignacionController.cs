using BelicoSysApp.Models;
using BelicoSysApp.Services;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Vml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Tls;
using System.Collections;
using System.Collections.Generic;
using System.Web.WebPages;

namespace BelicoSysApp.Controllers
{
    public class AsignacionController : Controller
    {

        private readonly IApiServiceAsignacion _apiServiceAsignacion;
        private readonly IApiServiceArma _apiServiceAarma;
        public AsignacionController(IApiServiceAsignacion apiServiceAsignacion, IApiServiceArma apiServiceAarma)
        {
            _apiServiceAsignacion = apiServiceAsignacion;
            _apiServiceAarma = apiServiceAarma;
        }
        public async Task<IActionResult> AsignacionReport()
        {
            ICollection<AsignacionArma> lista = await _apiServiceAsignacion.GetAsignaciones();

            // TODO: Get options from API
            // NOTE: When pagination added, filtering should be performed on the backend
            var ddRangoOptions = lista.Select(x => x.AsignacionRango).ToList().Distinct();

            //lista = lista.Take(30).ToList();

            ViewBag.Arma = lista;
            ViewBag.ddRangoOptions = ddRangoOptions;

            return View(lista);
        }

        [HttpGet]
        public async Task<IActionResult> SearchPeople(string? nombre, string? cedula)
        {
            string status = "A";
            IEnumerable<VPersonal> lista = await _apiServiceAsignacion.GetVPersonal(nombre, cedula, status);
            var listaDto = new List<VPersonal>();

            foreach (var personal in lista)
            {
                listaDto.Add(personal);
            }

            var data = new SelectList(listaDto);
            ViewBag.Pertrecho = new SelectList(listaDto, "MilitarNo", "Nombres");


            return Ok(listaDto);
        }
        [HttpGet]
        public async Task<IActionResult> SearchArma(string armaSerial)
        {
            VArma lista = await _apiServiceAsignacion.GetVArmaSerial(armaSerial);

            return Ok(lista);
        }

        [HttpGet]
        public async Task<IActionResult> SearchPeopleDoc(string nombre, string cedula)
        {
            string status = "A";
            IEnumerable<VPersonal> lista = await _apiServiceAsignacion.GetVPersonal(nombre, cedula, status);
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
        [HttpGet]
        public IActionResult AsignacionOrden()
        {



            return View();
        }
        [HttpGet]
        public IActionResult AsignacionOLista()
        {



            return View();
        }


        [HttpPost]
        public async Task<IActionResult> DescargoArma(VPersonal codigo)
        {
            var obtasig = codigo.MilitarNo;
            IEnumerable<AsignacionArma> lisAsig = _apiServiceAsignacion.GetAsignaciones().Result.Where(e => e.AsignacionNoRango == obtasig);
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
            ViewBag.Pertrecho = listaDto;

            VPersonal listaP = await _apiServiceAsignacion.GetVPersonaId(codigo.MilitarNo);

            ViewBag.Nombres = listaP.Nombres;
            ViewBag.desc_rango = listaP.desc_rango;
            ViewBag.Cedula = listaP.Cedula;
            ViewBag.Tel = listaP.num_celular;
            ViewBag.Dept = listaP.desc_departamento;
            ViewBag.noM = listaP.MilitarNo;




            ViewBag.DeleteMessage = "Registro Eliminado";




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
            ViewBag.pertrecho = new SelectList(listaDtop, "IdPertrechos", "PertrechosDescripcion");
            ViewBag.AsignacionEstado = new SelectList(listaEstDto, "IdAsignacionEstado", "AsignacionEstadoDescripcion");
            ViewBag.AsignacionTipo = new SelectList(listaEstDto, "IdAsignacionEstado", "AsignacionEstadoDescripcion");
            ViewBag.SuccessMessage = null;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AsignacionCreate(AsignacionArma model, string searchArmaInput)
        {
            model.AsignacionEstado = 1;
            ArmaController arma = new ArmaController();
            
            

            try
            {
                if (model.IdAsignacion == 0)
                {
                    ArmaUpdateDto consArma = new ArmaUpdateDto
                    {
                        idArma = model.IdArma.Value,
                        armaSerie = searchArmaInput,
                        armaEstado = 6
                    };

                     await _apiServiceAarma.Edit(consArma);
                    var respuesta = await _apiServiceAsignacion.Save(model);
                    if (respuesta.IdAsignacion == 0)
                    {
                        ModelState.AddModelError("", "Error el Numero de Serie ya esta registrado");
                    }
                    TempData["SuccessMessage"] = $"Registro Creado Con el ID {respuesta.IdAsignacion}";
                    ViewBag.SuccessMessage = "Item has been created successfully.";
                }
                else
                {
                    ModelState.AddModelError("", "Error Contacatar el Administrador");
                }

                return View();
            }
            catch (Exception e )
            {
               Console.WriteLine(e.ToString());
                return View();
            }

           
        }

        [HttpGet]
        public async Task<IActionResult> PeronaId(decimal id)
        {
            var itemPersona = await _apiServiceAsignacion.GetVPersonaId(id);

            if (itemPersona != null)
            {

                var itemPDto = new ApiResult();

                itemPDto.vPersonal = itemPersona;


                return Ok(itemPDto.vPersonal.Cedula);
            }
            return NotFound();
        }
        [HttpGet]
        public async Task<IActionResult> PeronaIdrango(decimal id)
        {
            var itemPersona = await _apiServiceAsignacion.GetVPersonaId(id);

            if (itemPersona != null)
            {

                var itemPDto = new ApiResult();

                itemPDto.vPersonal = itemPersona;


                return Ok(itemPDto.vPersonal.Rangos);
            }
            return NotFound();
        }
        [HttpGet]
        public async Task<IActionResult> PeronaIdNoMilitar(decimal id)
        {
            var itemPersona = await _apiServiceAsignacion.GetVPersonaId(id);

            if (itemPersona != null)
            {

                var itemPDto = new ApiResult();

                itemPDto.vPersonal = itemPersona;


                return Ok(itemPDto.vPersonal);
            }
            return NotFound();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var delete = await _apiServiceAsignacion.Delete(id);

            if (delete)
            {
                TempData["SuccessMessage"] = $"Registro Eliminado";
                return Ok("DescargoArma");
            }
            else
            {
                ModelState.AddModelError("", "Error Contacatar el Administrador");
            }
            return View("DescargoArma");
        }


        public IActionResult MenuAsignacion()
        {
            string successMessage = TempData["SuccessMessage"] as string;

            // Pass the success message to the view
            ViewBag.SuccessMessage = successMessage;
            return View();
        }
        [HttpPost]
        public  async Task<IActionResult> SaveOrder(Order data, IList<IFormFile> imageFile)
        {
            string imageRuta= "";
            if (imageFile != null && imageFile.Count > 0)
            {
                foreach (var file in imageFile)
                {
                    // Generate a unique file name to avoid conflicts
                    var fileName = Guid.NewGuid().ToString() + data.Orden_id.ToString() + System.IO.Path.GetExtension(file.FileName);
                    data.Document_File = fileName;
                    // Set the path where you want to save the image on the server
                    var imagePath = System.IO.Path.Combine("wwwroot", "Images", fileName);
                    imageRuta =imagePath.ToString();
                    // Save the image file to the server
                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
            }

            if (data.Orden_id == 0)
            {
                ICollection<Arma> filtro = await _apiServiceAarma.GetArmasMasiva(data.Orden_total,2);
                int ultimo = await _apiServiceAsignacion.GetOrderLastNumber() + 1;

                if (filtro != null) 
                {
                    foreach (var item in filtro) 
                    {
                        OrderDetalle oDetalle = new OrderDetalle
                        {
                            Orden_id = ultimo,
                            ArmaSerie = item.armaSerie
                           
                        };     
                        
                    await _apiServiceAsignacion.SaveOrderDetalle(oDetalle);
                   // await _apiServiceAarma.Edit
                    }
                    Console.WriteLine("detalle Guardado");
                }
                data.Document_File = imageRuta;
                var respuesta =  await _apiServiceAsignacion.SaveOrder(data);
                if (respuesta.Orden_id == 0)
                {
                    ModelState.AddModelError("", "Error el Numero de Serie ya esta registrado");
                }
                TempData["SuccessMessage"] = $"Registro Creado Con el ID {respuesta.Orden_id}";
                ViewBag.SuccessMessage = "Item has been created successfully.";
                return View("AsignacionOrden");
            }
            else
            {
                ModelState.AddModelError("", "Error Contacatar el Administrador");
                return Json("Error Contacatar el Administrador");
            }            

        }
        [HttpGet]
        public async Task<JsonResult> GetOrderNumber()
        {
            int lastOrderNumber = await _apiServiceAsignacion.GetOrderLastNumber();

            return Json($"{lastOrderNumber + 1}");
        }

        [HttpGet]

        public async Task<JsonResult> GetOrdenes() 
        {
            ICollection<Order> ordenes = await _apiServiceAsignacion.GetOrders();
            
            return Json(ordenes);
        }
        [HttpGet]
        public async Task<JsonResult> GetOrderInd(int inpCantidad)
        {
            var itemPersona = await _apiServiceAsignacion.GetOrderIndi(inpCantidad);

           
            return Json(itemPersona);
        }

        [HttpGet]
        public async Task<JsonResult> GetOrderDExiste(string serie ,int orderId)
        {
            var itemPersona = await _apiServiceAsignacion.ExisteFusil(serie,orderId);


            return Json(itemPersona);
        }

        [HttpGet]

        public async Task<JsonResult> GetAsignacionPertrechos()
        {
            ICollection<AsignacionPertrecho> asigPertrecho = await _apiServiceAsignacion.GetAsignacionesPertrecho();

            return Json(asigPertrecho);
        }

        [HttpPost]
        public async Task<IActionResult> AsignacionPertrechoCreate(AsignacionPertrecho model)
        {

            if (model.Id_Asignacion_pertrecho == 0)
            {
                var respuesta = await _apiServiceAsignacion.SaveAsigPertrecho(model);
                if (respuesta.Id_Asignacion_pertrecho == 0)
                {
                    ModelState.AddModelError("", "Error el Numero de Serie ya esta registrado");
                }
                //TempData["SuccessMessage"] = $"Registro Creado Con el ID {respuesta.Id_Asignacion_pertrecho}";
            }
            else
            {
                ModelState.AddModelError("", "Error Contacatar el Administrador");
            }

            return Ok();
        }

        [HttpGet]
        public async Task<JsonResult> GetasignacionPertrecho(int AsigP)
        {
            var itemPersona = await _apiServiceAsignacion.GetAsigPertrecho(AsigP);

            return Json(itemPersona);
        }



    }
}


