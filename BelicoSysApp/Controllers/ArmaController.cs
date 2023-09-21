using BelicoSysApp.Models;
using BelicoSysApp.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Net.Http;

namespace BelicoSysApp.Controllers
{
    public class ArmaController : Controller
    {
        private readonly IApiServiceArma _apiArma;
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
        public async Task<IActionResult> Export()
        {
            ICollection<VArma> lista = await _apiArma.GetVArmas();

            if (lista.Count != 0)
            {
                

               var valuesList = lista.ToList();
                

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                    // Add headers
                    worksheet.Cells[1, 1].Value = "Index";
                    worksheet.Cells[1, 2].Value = "Value";

                    // Add data
                    for (int i = 0; i < valuesList.Count; i++)
                    {
                        worksheet.Cells[i + 2, 1].Value = i + 1;
                        worksheet.Cells[i + 2, 2].Value = valuesList[i];
                    }

                    var stream = new MemoryStream(package.GetAsByteArray());

                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "values.xlsx");
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
