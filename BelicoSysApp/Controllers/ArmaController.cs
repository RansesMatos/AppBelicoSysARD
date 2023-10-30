using BelicoSysApp.Models;
using BelicoSysApp.Services;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using System.Collections;
using System.Drawing;
using System.Net.Http;

namespace BelicoSysApp.Controllers
{
    public class ArmaController : Controller
    {
        private readonly IApiServiceArma _apiArma;
        ArrayList multiimagename = new ArrayList();
        public ArmaController(IApiServiceArma apiArma)
        {
            _apiArma = apiArma;
        }

        
        public async Task<IActionResult> ExportPDf()
        {
            ICollection<VArma> lista = await _apiArma.GetVArmas();
            var valuesList = lista.ToList();

            string fileName = "values.pdf";

            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

                document.Open();
                PdfPTable table = new PdfPTable(2);

               // table.AddCell("Index");
               // table.AddCell("Value");

                //for (int i = 0; i < valueslist.count; i++)
                //{
                //    table.addcell((i + 1).tostring());
                //    table.addcell(valueslist[i].armaserie);
                //}
                foreach (var item in valuesList)
                {
                    table.AddCell(item.TaNombre);
                    table.AddCell(item.FechaRegistro.ToShortTimeString());
                    table.AddCell(item.AlmacenDescripcion);
                    table.AddCell(item.ArmaCalibre);
                    table.AddCell(item.ArmaStatus.ToString());
                    table.AddCell(item.ArmaSerie);
                    table.CompleteRow();


                }

                document.Add(table);
                document.Close();

                return File(memoryStream.ToArray(), "application/pdf", fileName);
            }
        }

        [HttpGet]
        public async Task<IActionResult> SearchArma(string armaSerial)
        {
            VArma lista = await _apiArma.GetVArmaSerial(armaSerial);

            return Ok(lista);
        }
        public async Task<IActionResult> Export()
        {
            ICollection<VArma> lista = await _apiArma.GetVArmas();

            if (lista.Count != 0)
            {
                

               var valuesList = lista.ToList();
                

                using (var package = new XLWorkbook())
                {
                    var worksheet = package.Worksheets.Add("Sheet1");

                    // Add headers
                    worksheet.Cell(1,1).Value = "Index";
                    worksheet.Cell(1, 2).Value = "Value";

                    // Add data
                    for (int i = 0; i < valuesList.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = i + 1;
                        worksheet.Cell(i + 2, 2).Value = valuesList[i].ToString();
                    }

                    var stream = new MemoryStream();
                    package.SaveAs(stream);

                    return File(stream, "application/xml", "values.xlsx");
                }
            }

            return BadRequest("Unable to fetch API data.");
        }

        public async Task<IActionResult> ArmaReport()
        {
            ICollection<VArma> lista = await _apiArma.GetVArmas();

            // Get filter options
            // TODO: Get options from API
            // NOTE: When pagination added, filtering should be performed on the backend
            var ddTipoArmaOptions = lista.Select(x => x.TaNombre).ToList().Distinct();
            var ddMarcaOptions = lista.Select(x => x.ArmaMarcaDescripcion).ToList().Distinct();
            var ddCalibreOptions = lista.Select(x => x.ArmaCalibre).ToList().Distinct();
            var ddAlmacenOptions = lista.Select(x => x.AlmacenDescripcion).ToList().Distinct();
            var ddEstadoOptions = lista.Select(x => x.ArmaEstadoDescripcion).ToList().Distinct();
            var ddEstatusOptions = new List<string>() { "Activado", "Desactivado" };

            //lista = lista.Take(20).ToList();

            ViewBag.Arma = lista;

            ViewBag.ddTipoArmaOptions = ddTipoArmaOptions;
            ViewBag.ddMarcaOptions = ddMarcaOptions;
            ViewBag.ddCalibreOptions = ddCalibreOptions;
            ViewBag.ddAlmacenOptions = ddAlmacenOptions;
            ViewBag.ddEstadoOptions = ddEstadoOptions;
            ViewBag.ddEstatusOptions = ddEstatusOptions;


            return View(lista);
        }

        [HttpGet]
        public async Task<IActionResult> ArmaCreate()
        {
            IEnumerable<Almacen> listaA = await _apiArma.GetAlmacenes();
            IEnumerable<ArmaMarca> listaM = await _apiArma.GetArmasMarcas();
            IEnumerable<TipoArma> listaAT = await _apiArma.GetArmasTipos();
            var listaADto = new List<Almacen>();
            var listaMarcaDto = new List<ArmaMarca>();
            var listaTipoDto = new List<TipoArma>();
            foreach (var Almacen in listaA)
            {
                listaADto.Add(Almacen);
            }

            var data = new SelectList(listaADto);
            // ViewBag.pertrecho = new SelectList(listaDto, "PertrechosDescripcion", "Cantidad");
            foreach (var marca in listaM)
            {
                listaMarcaDto.Add(marca);
            }
            foreach (var tipoArma in listaAT)
            {
                listaTipoDto.Add(tipoArma);
            }

            ViewBag.Almacen = new SelectList(listaADto, "IdAlmacen", "AlmacenDescripcion");
            ViewBag.Marca = new SelectList(listaMarcaDto, "IdArmaMarca", "ArmaMarcaDescripcion");
            ViewBag.ArmaTipo = new SelectList(listaTipoDto, "IdTipoArma", "TaNombre");
            


            return View("ArmaCreate");
        }
        [HttpPost]
        public async Task<IActionResult> ArmaCreate(Arma model, IList<IFormFile> imageFile)
        {
            multiimagename.Clear();
            if (imageFile != null && imageFile.Count > 0 || imageFile.Count < 4)
            {
                foreach (var file in imageFile)
                {
                    // Generate a unique file name to avoid conflicts
                    var fileName = Guid.NewGuid().ToString() + model.ArmaSerie.ToString() + Path.GetExtension(file.FileName);
                    model.ImagePath = fileName;
                    // Set the path where you want to save the image on the server
                    var imagePath = Path.Combine("wwwroot", "Images", fileName);
                    multiimagename.Add(imagePath.ToString());
                    // Save the image file to the server
                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
            }

            if (model.IdArma == 0)
            {
                model.ImagePath = multiimagename[0] + ";" + multiimagename[1] + ";" + multiimagename[2] + ";" + multiimagename[3];
                var respuesta = await _apiArma.Save(model);
                if (respuesta.IdArma == 0)
                {
                    ModelState.AddModelError("", "Error el Numero de Serie ya esta registrado");
                }
                Zen.Barcode.Code128BarcodeDraw armaBarcode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                var codigoB = armaBarcode.Draw(respuesta.ArmaSerie,60);

                //var imagePath = Path.Combine("wwwroot", "Images", );
                //using (var fileStream = new Bitmap(imagePath, FileMode.Create))
                //{
                //    file.CopyTo(fileStream);
                //}
                TempData["SuccessMessage"] = $"Registro Creado Con el ID {respuesta.IdArma}";
                ViewBag.SuccessMessage = $"Arma Registrada Con el ID {respuesta.IdArma}";
            }
            else
            {
                ModelState.AddModelError("", "Error Contacatar el Administrador");
            }

            return View("ArmaCreate");
        }


        [HttpGet]
        public async Task<IActionResult> ArmaUpdate()
        {
            IEnumerable<Almacen> listaA = await _apiArma.GetAlmacenes();
            IEnumerable<ArmaMarca> listaM = await _apiArma.GetArmasMarcas();
            IEnumerable<TipoArma> listaAT = await _apiArma.GetArmasTipos();
            var listaADto = new List<Almacen>();
            var listaMarcaDto = new List<ArmaMarca>();
            var listaTipoDto = new List<TipoArma>();
            foreach (var Almacen in listaA)
            {
                listaADto.Add(Almacen);
            }

            var data = new SelectList(listaADto);
            // ViewBag.pertrecho = new SelectList(listaDto, "PertrechosDescripcion", "Cantidad");
            foreach (var marca in listaM)
            {
                listaMarcaDto.Add(marca);
            }
            foreach (var tipoArma in listaAT)
            {
                listaTipoDto.Add(tipoArma);
            }

            ViewBag.Almacen = new SelectList(listaADto, "IdAlmacen", "AlmacenDescripcion");
            ViewBag.Marca = new SelectList(listaMarcaDto, "IdArmaMarca", "ArmaMarcaDescripcion");
            ViewBag.ArmaTipo = new SelectList(listaTipoDto, "IdTipoArma", "TaNombre");



            return View();
        }


        [HttpGet]
        public IActionResult MenuArma()

        {
            string successMessage = TempData["SuccessMessage"] as string;

            // Pass the success message to the view
            ViewBag.SuccessMessage = successMessage;
            return View();
        }



    }

}
