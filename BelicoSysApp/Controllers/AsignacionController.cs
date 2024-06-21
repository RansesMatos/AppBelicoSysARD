using BelicoSysApp.Models;
using BelicoSysApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BelicoSysApp.Controllers
{
    public class AsignacionController : Controller
    {
       

        private readonly IApiServiceAsignacion _apiServiceAsignacion;
        private readonly IApiServiceArma _apiServiceAarma;
        private readonly IApiServicePertrecho _apiPertrecho;
        public AsignacionController(IApiServiceAsignacion apiServiceAsignacion, IApiServiceArma apiServiceAarma, IApiServicePertrecho apiPertrecho)
        {
            _apiServiceAsignacion = apiServiceAsignacion;
            _apiServiceAarma = apiServiceAarma;
            _apiPertrecho = apiPertrecho;

        
        }

        public async Task<ActionResult> GenerateDesasignacionCertificationPdf(int selectedOption)
        {
            // Obtener datos necesarios del usuario y los ítems asignados
            VPersonal userInfo = await _apiServiceAsignacion.GetVPersonaId(selectedOption);
            IEnumerable<AsignacionArma> asignaciones = await _apiServiceAsignacion.GetAsignaciones();
            IEnumerable<VPertrecho> pertrechosAsignados = await _apiServiceAsignacion.GetVPertrechos();

            // Filtrar datos específicos para la desasignación
            var datosDesasignados = asignaciones.Where(x => x.AsignacionNoRango == selectedOption && !x.AsignacionStatus).ToList();
            var pertrechosDesasignados = pertrechosAsignados.Where(x => x.Id_Militar == selectedOption && !x.status).ToList();

            // Crear el PDF
            using (MemoryStream ms = new MemoryStream())
            {
                Document pdf = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(pdf, ms);
                pdf.Open();

                // Agregar contenido al PDF
                AddPdfHeader(pdf, "CERTIFICADO DE DESASIGNACIÓN");
                AddPdfParagraph(pdf, $"Este certificado verifica que los siguientes ítems han sido desasignados de {userInfo.Nombres}:");

                // Listar armas desasignadas
                foreach (var item in datosDesasignados)
                {
                    string armaDetails = $"Arma: {item.IdArma}";
                    AddPdfParagraph(pdf, armaDetails);
                }

                // Listar pertrechos desasignados
                foreach (var item in pertrechosDesasignados)
                {
                    string pertrechoDetails = $"Pertrecho: {item.pertrechos_descripcion} - Cantidad: {item.cantidad}";
                    AddPdfParagraph(pdf, pertrechoDetails);
                }

                // Información adicional y firma
                AddPdfSignature(pdf, userInfo);

                pdf.Close();
                writer.Close();

                // Guardar el PDF en un archivo y/o enviarlo al usuario
                return File(ms.ToArray(), "application/pdf", $"Desasignacion-{selectedOption}.pdf");
            }
        }

        // Métodos auxiliares para añadir contenido al PDF
        private void AddPdfHeader(Document document, string header)
        {
            Paragraph headerParagraph = new Paragraph(header, new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD));
            headerParagraph.Alignment = Element.ALIGN_CENTER;
            document.Add(headerParagraph);
        }

        private void AddPdfParagraph(Document document, string content)
        {
            Paragraph paragraph = new Paragraph(content, new Font(Font.FontFamily.HELVETICA, 10));
            paragraph.Alignment = Element.ALIGN_LEFT;
            document.Add(paragraph);
        }

        private void AddPdfSignature(Document document, VPersonal userInfo)
        {
            Paragraph signature = new Paragraph($"Firmado por: {userInfo.Nombres}, {userInfo.Rangos}", new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD));
            signature.Alignment = Element.ALIGN_RIGHT;
            document.Add(signature);
        }


        [HttpGet]
        public async Task<IActionResult> DescargarPertrecho()
        {
            ViewBag.count = 0;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DescargarPertrecho(int idPertrecho , int idMilitar)
        {
         var pertrechosMilitar = await _apiServiceAsignacion.GetAsigPertrecho(idPertrecho, idMilitar);

            if (pertrechosMilitar != null) {

               var pertrecho = await _apiPertrecho.Get(pertrechosMilitar.Id_pertrechos);

                if (pertrecho.IdPertrechos == idPertrecho)
                {
                    await _apiServiceAsignacion.ActualizarPertrecho(pertrecho);
                }
            }

            return View();
        }
  
        public async Task<IActionResult> AsignacionReport()
        {
            ICollection<AsignacionArma> lista = await _apiServiceAsignacion.GetAsignaciones();
            List<AsignacionArma> updatedList = new List<AsignacionArma>();

            foreach (var item in lista) 
            {
                var query = await _apiServiceAsignacion.GetVPersonaId(Convert.ToDecimal(item.AsignacionDocumento));
                item.StatusARD = query.desc_estatus;
                updatedList.Add(item);
            }

            lista = updatedList;

            var statARD = updatedList.Select(x => x.StatusARD).ToList().Distinct();


            // TODO: Get options from API
            // NOTE: When pagination added, filtering should be performed on the backend
            var ddRangoOptions = lista.Select(x => x.AsignacionRango).ToList().Distinct();


            

            //lista = lista.Take(30).ToList();

            ViewBag.Arma = lista;
            ViewBag.ddRangoOptions = ddRangoOptions;
            ViewBag.ddRestatusOptions = statARD;


            return View(lista);
        }
        [HttpGet]
        public async Task<IActionResult> GetAsignaciones(int noRango)
        {
            
            IEnumerable<AsignacionArma> lista = await _apiServiceAsignacion.GetAsignaciones();
            var datos = lista.Where(x=> x.AsignacionNoRango == noRango);

            var listaDto = new List<AsignacionArma>();

            foreach (var asignacion in datos)
            {
                listaDto.Add(asignacion);
            }


            return Ok(listaDto);
        }
        [HttpGet]
        public async Task<IActionResult> GetVarmaAll(int idarma)
        {

            IEnumerable<VArma> lista = await _apiServiceAarma.GetVArmas();
            var datos = lista.Where(x => x.IdArma == idarma);

            var listaDto = new List<VArma>();

            foreach (var asignacion in datos.Distinct().ToList())
            {
                
                    listaDto.Add(asignacion);
                
            }

            List<VArma> listafilter = listaDto
                .Where(x => x.IdArma == idarma)
                .GroupBy(x => x.IdArma)  // Agrupar por IdArma para manejar duplicados
                .Select(g => g.First())  // Seleccionar el primer elemento de cada grupo
                .ToList();


            return Ok(listafilter);
        }

        [HttpGet]
        public async Task<IActionResult> SearchPeople(string? carnet, string? cedula)
        {
            string status = "A";
            IEnumerable<VPersonal> lista = await _apiServiceAsignacion.GetVPersonal(carnet, cedula, status);
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
        public async  Task<IActionResult> AsignacionOrden()
        {
            IEnumerable<TipoArma> listaAT = await _apiServiceAarma.GetArmasTipos();
            var listaTipoDto = new List<TipoArma>();
            foreach (var tipoArma in listaAT)
            {
                listaTipoDto.Add(tipoArma);
            }
            ViewBag.ArmaTipo = new SelectList(listaTipoDto, "IdTipoArma", "TaNombre");


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
            IEnumerable<AsignacionArma> lisAsig = _apiServiceAsignacion.GetAsignaciones().Result;
            lisAsig = lisAsig.Where(x=> x.AsignacionNoRango == obtasig && x.AsignacionStatus && x.AsignacionEstado == 1 ).Distinct();
            IEnumerable<VArma> lista = await _apiServiceAsignacion.GetVArmas();
            var listaDto = new List<VArma>();
            var listaasigDto = new List<AsignacionArma>();
            int? valorarma = 0;

        

            foreach (var asig in lisAsig)
            {
                listaasigDto.Add(asig);
                valorarma = asig.IdArma;
                foreach (var arma in lista.Where(e => e.IdArma == valorarma).Distinct())
                {
                    if(listaDto.Count < listaasigDto.Count) { 
                    listaDto.Add(arma);
                        }
                }
            }

            //==========================Petrechos
            IEnumerable<VPertrecho> listaPer = _apiServiceAsignacion.GetVPertrechos().Result;
            listaPer = listaPer.Where(x => x.Id_Militar == obtasig && x.status == true);
            //foreach (var arma in lista.Where(e => e.IdArma == nuevoC))
            //{
            //    listaDto.Add(arma);
            //}
            ViewBag.count = listaasigDto.Count;
            ViewBag.Arma = listaDto;
            ViewBag.Pertrecho = listaPer;
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
            try
            {
                if (model.IdAsignacion == 0)
                {
                    ArmaUpdateDto consArma = new ArmaUpdateDto
                    {
                        idArma = model.IdArma.Value,
                        armaSerie = searchArmaInput,
                        armaEstado = 1

                    };
                    if (await _apiServiceAarma.Edit(consArma))
                    {
                         var listAsignacion = await  _apiServiceAsignacion.GetAsignaciones();
                        var valvalidarArma = listAsignacion.Where(x => x.IdArma == model.IdArma && x.AsignacionStatus && x.AsignacionEstado == 1);
                        if (valvalidarArma == null)
                        {
                            var respuesta = await _apiServiceAsignacion.Save(model);

                            if (respuesta.IdAsignacion == 0)
                            {
                                ModelState.AddModelError("", "Error el Numero de Serie ya esta registrado");
                            }
                            TempData["SuccessMessage"] = $"Registro Creado Con el ID {respuesta.IdAsignacion}";
                            ViewBag.SuccessMessage = "Item has been created successfully.";
                        }

                        else { 

                            ModelState.AddModelError("", "Error el Numero de Serie ya esta registrado");
                    }
                    }
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

      //  [HttpGet]
        //public async Task<JsonResult> GetasignacionPertrecho(int AsigP)
        //{
        //    var itemPersona = await _apiServiceAsignacion.GetAsigPertrecho(AsigP);

        //    return Json(itemPersona);
        //}

        [HttpGet]
        public async Task<JsonResult> SearchArmaJson(string armaSerial)
        {
            VArma lista = await _apiServiceAsignacion.GetVArmaSerial(armaSerial);

            return Json(lista);
        }

        [HttpGet]

        public async Task<JsonResult> GetAsignacionPertrechosM(int militarNo)
        {
            IEnumerable<VPertrecho> asigPertrecho = await _apiServiceAsignacion.GetVPertrechos();
            
            var datos = asigPertrecho.Where(x => x.Id_Militar == militarNo && x.status);

            var listaDto = new List<VPertrecho>();

            foreach (var asignacion in datos)
            {
                listaDto.Add(asignacion);
            }

            return Json(listaDto);
        }


    }

}


