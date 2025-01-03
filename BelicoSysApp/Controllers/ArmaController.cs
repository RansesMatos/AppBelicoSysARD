﻿using BelicoSysApp.Models;
using BelicoSysApp.Services;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections;
using System.Drawing;


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


        public async Task<IActionResult> ExportPDF()
        {
            // Obtener datos
            ICollection<VArma> lista = await _apiArma.GetVArmas();
            var valuesList = lista.ToList();
            string fileName = "values.pdf";

            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Configuración inicial del documento
                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                // Crear la tabla con el número correcto de columnas
                PdfPTable table = new PdfPTable(6);  // Ajusta según el número de columnas necesarias

                // Añadir headers a la tabla
                table.AddCell("Nombre del Tipo de Arma");
                table.AddCell("Fecha de Registro");
                table.AddCell("Descripción del Almacén");
                table.AddCell("Calibre del Arma");
                table.AddCell("Estado del Arma");
                table.AddCell("Serie del Arma");

                // Llenar la tabla con datos
                foreach (var item in valuesList)
                {
                    table.AddCell(item.TaNombre);
                    table.AddCell(item.FechaRegistro.ToString("g")); // Cambiado para incluir fecha y hora
                    table.AddCell(item.AlmacenDescripcion);
                    table.AddCell(item.ArmaCalibre);
                    table.AddCell(item.ArmaStatus.ToString());
                    table.AddCell(item.ArmaSerie);

                    table.AddCell(item.BarcodePath);
                    table.CompleteRow();

                }

                // Añadir la tabla al documento
                document.Add(table);
                document.Close();


                return File(memoryStream.ToArray(), "application/pdf", fileName);
            }
        }

        //public async Task<IActionResult> ExportArmaToExcel()
        //{ }

        [HttpGet]
        public async Task<IActionResult> ExportExcel()
        {
            // Obtener datos de la base de datos usando EF Core
            var arma = await _apiArma.GetVArmas();
            var valuesList = arma.ToList();

            // Crear un nuevo paquete de Excel
            using (var package = new ExcelPackage())
            {
                // Agregar una nueva hoja de cálculo al libro vacío
                var worksheet = package.Workbook.Worksheets.Add("Reporte");

                // Cargar los datos en la hoja de cálculo
                worksheet.Cells["A1"].Value = "Id_Arma";
                worksheet.Cells["B1"].Value = "Nombre Arma";
                worksheet.Cells["C1"].Value = "Descripcion";
                worksheet.Cells["D1"].Value = "Calibre";
                worksheet.Cells["E1"].Value = "Serie";
                worksheet.Cells["F1"].Value = "Almacen";
                worksheet.Cells["G1"].Value = "ArmaStatus";    
                worksheet.Cells["H1"].Value = "Image_1";
                worksheet.Cells["I1"].Value = "Image_2";
                worksheet.Cells["J1"].Value = "Image_3";
                worksheet.Cells["K1"].Value = "Image_4";
                worksheet.Cells["M1"].Value = "Codigo de Barra";
             

                for (int i = 0; i < valuesList.Count; i++)
                {
                    var armaItem = valuesList[i];

                    worksheet.Cells[i + 2, 1].Value = armaItem.IdArma;
                    worksheet.Cells[i + 2, 2].Value = armaItem.TaNombre;
                    worksheet.Cells[i + 2, 3].Value = armaItem.ArmaMarcaDescripcion;
                    worksheet.Cells[i + 2, 4].Value = armaItem.ArmaCalibre;
                    worksheet.Cells[i + 2, 5].Value = armaItem.ArmaSerie;
                    worksheet.Cells[i + 2, 6].Value = armaItem.AlmacenDescripcion;
                    worksheet.Cells[i + 2, 7].Value = armaItem.AlmacenDescripcion;
                    worksheet.Cells[i + 2, 8].Value = armaItem.ImagePath1 ?? string.Empty;
                    worksheet.Cells[i + 2, 9].Value = armaItem.ImagePath2 ?? string.Empty;
                    worksheet.Cells[i + 2, 10].Value = armaItem.ImagePath3 ?? string.Empty;
                    worksheet.Cells[i + 2, 11].Value = armaItem.ImagePath4 ?? string.Empty;
                    worksheet.Cells[i + 2, 12].Value = armaItem.BarcodePath ?? string.Empty;
                }

                // Formatear el encabezado
                using (var range = worksheet.Cells[1, 1, 1, 16])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                    range.Style.Font.Color.SetColor(Color.White);
                }

                // Ajustar el ancho de las columnas
                worksheet.Cells.AutoFitColumns();

                // Guardar el paquete de Excel en un MemoryStream
                var stream = new MemoryStream();
                package.SaveAs(stream);

                // Devolver el archivo Excel como un archivo descargable
                stream.Position = 0;
                var fileName = "Reporte.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

            }
        }

        public async Task<IActionResult> ExportOneArma(string ParameterSerie)
        {
            // Obtener datos de la base de datos usando EF Core
            var arma = await _apiArma.GetVArmaSerialList(ParameterSerie);

            // Asegurarse de que el objeto no sea nulo
            if (arma == null)
            {
                return NotFound();
            }

            var armas = arma.FirstOrDefault();

            // Crear un nuevo paquete de Excel
            using (var package = new ExcelPackage())
            {
                // Agregar una nueva hoja de cálculo al libro vacío
                var worksheet = package.Workbook.Worksheets.Add("Reporte");

                // Cargar los datos en la hoja de cálculo
                worksheet.Cells["A1"].Value = "Id_Arma";
                worksheet.Cells["B1"].Value = "Nombre Arma";
                worksheet.Cells["C1"].Value = "Descripcion";
                worksheet.Cells["D1"].Value = "Calibre";
                worksheet.Cells["E1"].Value = "Serie";
                worksheet.Cells["F1"].Value = "Almacen";
                worksheet.Cells["G1"].Value = "ArmaStatus";
                worksheet.Cells["H1"].Value = "Image_1";
                worksheet.Cells["I1"].Value = "Image_2";
                worksheet.Cells["J1"].Value = "Image_3";
                worksheet.Cells["K1"].Value = "Image_4";
                worksheet.Cells["L1"].Value = "Codigo de Barra";

                worksheet.Cells[2, 1].Value = armas.IdArma;
                worksheet.Cells[2, 2].Value = armas.TaNombre;
                worksheet.Cells[2, 3].Value = armas.ArmaMarcaDescripcion;
                worksheet.Cells[2, 4].Value = armas.ArmaCalibre;
                worksheet.Cells[2, 5].Value = armas.ArmaSerie;
                worksheet.Cells[2, 6].Value = armas.AlmacenDescripcion;
                worksheet.Cells[2, 7].Value = armas.ArmaStatus;
                worksheet.Cells[2, 8].Value = armas.ImagePath1 ?? string.Empty;
                worksheet.Cells[2, 9].Value = armas.ImagePath2 ?? string.Empty;
                worksheet.Cells[2, 10].Value = armas.ImagePath3 ?? string.Empty;
                worksheet.Cells[2, 11].Value = armas.ImagePath4 ?? string.Empty;
                worksheet.Cells[2, 12].Value = armas.BarcodePath ?? string.Empty;

                // Formatear el encabezado
                using (var range = worksheet.Cells[1, 1, 1, 12])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                    range.Style.Font.Color.SetColor(Color.White);
                }

                // Ajustar el ancho de las columnas
                worksheet.Cells.AutoFitColumns();

                // Guardar el paquete de Excel en un MemoryStream
                var stream = new MemoryStream();
                package.SaveAs(stream);

                // Devolver el archivo Excel como un archivo descargable
                stream.Position = 0;
                var fileName = "Reporte.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        [HttpGet]
        //public async Task<IActionResult> SearchArma(string serie)
        //{
        //    //if (string.IsNullOrEmpty(armaSerial))
        //    //{
        //    //    ViewBag.Message = "Serie is null or empty.";
        //    //    return BadRequest();
        //    //}

        //    var arma = await _apiArma.GetVArmaSerial(serie);


        //    ViewBag.VArma = arma;

        //    return Ok(arma);
        //}
        public async Task<IActionResult> SearchArma(string serie)
        {
            ///Este es el metodo para trabajar con las 
            // Llama al método GetVArmaSerial para obtener los datos
           
                var armas = await _apiArma.GetVArmaSerial(serie);
                if (armas == null)
                {
                    ViewBag.Message = "No se encontraron datos para la serie proporcionada.";
                    return PartialView("_ArmaReport", new List<VArma>());
                }
                return PartialView("_ArmaReport", armas);  
        }

        [HttpGet]
        public async Task<JsonResult> SearchArmaAll(int cantidad,int cantidad2)
        {
            try
            {
                ICollection<Arma> armas = await _apiArma.GetArmasMasiva(cantidad, cantidad2);
                if (armas.Count < cantidad)
                {
                    ModelState.AddModelError("", $"No se tiene disponible esa cantidad la cantidad disponible es {armas.Count}");
                }

                return Json(armas);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", $"Hay un error consultado ");
                return Json(e);
            }
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
                    worksheet.Cell(1, 1).Value = "Index";
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
            IEnumerable<ArmaModelo> listaM0 = await _apiArma.GetArmasModelo();

            IEnumerable<TipoArma> listaAT = await _apiArma.GetArmasTipos();
            var listaADto = new List<Almacen>();
            var listaMarcaDto = new List<ArmaMarca>();
            var listaModeloDto = new List<ArmaModelo>();
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
            foreach (var modelo in listaM0)
            {
                listaModeloDto.Add(modelo);
            }
            foreach (var tipoArma in listaAT)
            {
                listaTipoDto.Add(tipoArma);
            }

            ViewBag.Almacen = new SelectList(listaADto, "IdAlmacen", "AlmacenDescripcion");
            ViewBag.Marca = new SelectList(listaMarcaDto, "IdArmaMarca", "ArmaMarcaDescripcion");
            ViewBag.Modelo = new SelectList(listaModeloDto, "idArmaModelo", "descModelo");
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
                    var fileName = Guid.NewGuid().ToString() + model.armaSerie.ToString() + Path.GetExtension(file.FileName);
                    model.ImagePath1 = fileName;
                    // Set the path where you want to save the image on the server
                    var imagePath = Path.Combine("wwwroot", "Images", fileName);
                    multiimagename.Add(fileName.ToString());
                    // Save the image file to the server
                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
            }

            if (model.idArma == 0)
            {
                Zen.Barcode.Code128BarcodeDraw armaBarcode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                var codigoB = armaBarcode.Draw(model.armaSerie, 120);

                var fileName = Guid.NewGuid().ToString() + model.armaSerie.ToString() + Path.GetExtension(".jpg");
                model.BarcodePath = fileName;
                var imagePath = Path.Combine("wwwroot", "Images", fileName);

                //using (var fileStream = new FileStream(imagePath, FileMode.Create))
                //{
                //    codigoB.Save(imagePath);
                //}
                codigoB.Save(imagePath);
               
                model.ImagePath1 = multiimagename[0].ToString();
                model.ImagePath2 = multiimagename[1].ToString();
                model.ImagePath3 = multiimagename[2].ToString();
                model.ImagePath4 = multiimagename[3].ToString();
                model.idModelo = 1;
                var respuesta = await _apiArma.Save(model);
                if (respuesta.idArma == 0)
                {
                    ModelState.AddModelError("", "Error el Numero de Serie ya esta registrado");
                }
                
                TempData["SuccessMessage"] = $"Registro Creado Con el ID {respuesta.idArma}";
                ViewBag.SuccessMessage = $"Arma Registrada Con el ID {respuesta.idArma}";
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
            IEnumerable<ArmaModelo> listaM0 = await _apiArma.GetArmasModelo();
            IEnumerable<TipoArma> listaAT = await _apiArma.GetArmasTipos();
            var listaADto = new List<Almacen>();
            var listaMarcaDto = new List<ArmaMarca>();
            var listaModeloDto = new List<ArmaModelo>();
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
            foreach (var modelo in listaM0)
            {
                listaModeloDto.Add(modelo);
            }
            foreach (var tipoArma in listaAT)
            {
                listaTipoDto.Add(tipoArma);
            }

            ViewBag.Almacen = new SelectList(listaADto, "IdAlmacen", "AlmacenDescripcion");
            ViewBag.Marca = new SelectList(listaMarcaDto, "IdArmaMarca", "ArmaMarcaDescripcion");
            ViewBag.Modelo = new SelectList(listaMarcaDto, "IdArmaModelo", "descModelo");
            ViewBag.ArmaTipo = new SelectList(listaTipoDto, "IdTipoArma", "TaNombre");

            return View();
        }

        [HttpPatch]
        public async Task<bool> ArmaUpdate(ArmaUpdateDto armaUpdateDto)
        {
            if (armaUpdateDto == null)
            {
                ModelState.AddModelError("", "Error Contacatar el Administrador");
            }
            if (armaUpdateDto.idArma != 0 && armaUpdateDto.armaSerie != null) {
               

                var armau = await _apiArma.Edit(armaUpdateDto);
                if (!armau) 
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
            
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
