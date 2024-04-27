using AspNetCore.Reporting;
using BelicoSysApp.Models;
using BelicoSysApp.Services;
using DinkToPdf;
using DinkToPdf.Contracts;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;

using System.Reflection;
using System.Web.WebPages;


namespace BelicoSysApp.Controllers
{
    public class CertificationController : Controller
    {
        private readonly IApiServiceAsignacion _apiServiceAsignacion;
        private readonly IApiServiceArma _apiServiceArma;
        private readonly IApiServicePertrecho _apiServicePertrecho;
        private readonly IConverter _converter;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CertificationController(IConverter converter, IApiServiceAsignacion apiServiceAsignacion, IWebHostEnvironment webHostEnvironment, IApiServiceArma apiServiceArma, IApiServicePertrecho apiServicePertrecho)
        {
            _converter = converter;
            _apiServiceAsignacion = apiServiceAsignacion;
            _apiServicePertrecho =  apiServicePertrecho;
            _apiServiceArma =  apiServiceArma ;
            _webHostEnvironment = webHostEnvironment;
        }
        // GET: CertificationController
        [HttpGet]
        public async Task<ActionResult> CertificationS(int selectedOption)
        {
            VPersonal listaP = await _apiServiceAsignacion.GetVPersonaId(selectedOption);
            IEnumerable<AsignacionArma> Lasig = await _apiServiceAsignacion.GetAsignaciones();
            IEnumerable<VArma> LArma = await _apiServiceArma.GetVArmas();
            IEnumerable<VPertrecho> lPert = await _apiServiceAsignacion.GetVPertrechos();
            var datosPert = lPert.Where(x => x.Id_Militar == selectedOption && x.status).ToList();
            var listaArmaDto = new List<VArma>();
            var datos = Lasig.Where(x => x.AsignacionNoRango == selectedOption).ToList();
            var listaDto = new List<AsignacionArma>();
            List<VArma> listafilter = new List<VArma>();


            foreach (var asignacion in datos)
            {
                listaDto.Add(asignacion);

                foreach (var arma in LArma.Where(x => x.IdArma == asignacion.IdArma))
                {
                    listaArmaDto.Add(arma);
                    listafilter = listaArmaDto
                       .Where(x => x.IdArma == arma.IdArma)
                   .GroupBy(x => x.IdArma)  // Agrupar por IdArma para manejar duplicados
              .Select(g => g.First())  // Seleccionar el primer elemento de cada grupo
              .ToList();
                }



            }
            using (MemoryStream ms = new MemoryStream())
            {

                Rectangle rect = new Rectangle(150, 200);
                rect.BorderColor = BaseColor.BLACK;
                rect.Border = Rectangle.BOX;
                rect.BorderWidth = 2;
                Document pdf = new Document(PageSize.A4);

                PdfWriter writer = PdfWriter.GetInstance(pdf, ms);


                pdf.AddAuthor("Armada");
                pdf.AddCreator("BelisoSys");
                pdf.AddSubject("ManejoBelico");
                pdf.AddTitle("BelisoSys");

                pdf.Open();
                Paragraph subHeader1 = new Paragraph("", new Font(Font.FontFamily.HELVETICA, 7));
                subHeader1.Alignment = Element.ALIGN_LEFT;
                pdf.Add(subHeader1);

                Paragraph subHeader2 = new Paragraph("INTENDENCIA MATERIAL BÉLICO", new Font(Font.FontFamily.HELVETICA, 7, Font.BOLD));
                subHeader2.Alignment = Element.ALIGN_CENTER;
                pdf.Add(subHeader2);
                Paragraph subHeader3 = new Paragraph("ARMADA DE REPÚBLICA DOMINICANA", new Font(Font.FontFamily.HELVETICA, 7));
                subHeader3.Alignment = Element.ALIGN_CENTER;
                pdf.Add(subHeader3);
                Paragraph subHeader4 = new Paragraph("Santo Domingo, Este.", new Font(Font.FontFamily.HELVETICA, 7));
                subHeader4.Alignment = Element.ALIGN_RIGHT;
                pdf.Add(subHeader4);
                Paragraph subHeader7 = new Paragraph(DateTime.Now.ToString("d MMMM yyyy", new System.Globalization.CultureInfo("es-ES")), new Font(Font.FontFamily.HELVETICA, 7));
                subHeader7.Alignment = Element.ALIGN_RIGHT;
                pdf.Add(subHeader7);
                Paragraph subHeader6 = new Paragraph("______________________________CERTIFICACION____________________________", new Font(Font.FontFamily.HELVETICA, 7));
                subHeader6.Alignment = Element.ALIGN_LEFT;
                pdf.Add(subHeader6);
                Paragraph subHeader8 = new Paragraph("Fecha", new Font(Font.FontFamily.HELVETICA, 7));
                subHeader8.Alignment = Element.ALIGN_CENTER;
                pdf.Add(subHeader8);
                pdf.Add(subHeader6);
                Paragraph subHeader9 = new Paragraph("POR MEDIO DE LA PRESENTE CERTIFICO QUE LA " + item.TaNombre + " MARCA" + item.ArmaMarcaDescripcion + " CAL." + item.ArmaCalibre + " NO." + item.ArmaSerie, new Font(Font.FontFamily.HELVETICA, 7));
                subHeader9.Alignment = Element.ALIGN_LEFT;
                pdf.Add(subHeader9);
                pdf.Add(subHeader6);
                Paragraph subHeader10 = new Paragraph("ES PROPIEDAD DE ESTA INSTITUCIÓN Y ESTÁ CARGADA COMO ARMA DE REGLAMENTO AL  "+ listaP.Rangos + listaP.Nombres +", ARD., ", new Font(Font.FontFamily.HELVETICA, 7));
                subHeader10.Alignment = Element.ALIGN_LEFT;
                pdf.Add(subHeader10);
                Paragraph subHeader11 = new Paragraph("propiedades de la Armada de la ARD y me comprometo a devolverlos cuando sean requeridos por la autoridad competente.", new Font(Font.FontFamily.HELVETICA, 7));
                subHeader11.Alignment = Element.ALIGN_LEFT;
                pdf.Add(subHeader11);
                pdf.Add(subHeader6);
                Paragraph subHeader12 = new Paragraph("CANTIDAD    ARTICULO    VALOR", new Font(Font.FontFamily.HELVETICA, 7, Font.BOLD));
                subHeader12.Alignment = Element.ALIGN_LEFT;
                pdf.Add(subHeader12);
                pdf.Add(subHeader6);

                foreach (var item in listafilter)
                {
                    Paragraph _par = new();

                    _par = new Paragraph("01.-" + item.TaNombre + " MARCA" + item.ArmaMarcaDescripcion + " CAL." + item.ArmaCalibre + " NO." + item.ArmaSerie, new Font(Font.FontFamily.HELVETICA, 7));
                    _par.Alignment = Element.ALIGN_LEFT;
                    pdf.Add(_par);

                }
                foreach (var item in datosPert)
                {
                    Paragraph _par = new();

                    _par = new Paragraph("0" + item.cantidad + " " + item.pertrechos_descripcion, new Font(Font.FontFamily.HELVETICA, 7));
                    _par.Alignment = Element.ALIGN_LEFT;
                    pdf.Add(_par);

                }

                Paragraph subHeader13 = new Paragraph("-x-x-x-x-x-x-x-x-x-x-x-x--x-x-x-x-x-x-x-x-x-x-x-x-x-", new Font(Font.FontFamily.HELVETICA, 7));
                subHeader13.Alignment = Element.ALIGN_LEFT;
                pdf.Add(subHeader13);
                Paragraph subHeader14 = new Paragraph(listaP.Nombres, new Font(Font.FontFamily.HELVETICA, 7, Font.BOLD));
                subHeader14.Alignment = Element.ALIGN_CENTER;
                subHeader14.SpacingBefore = 25f;
                pdf.Add(subHeader14);
                Paragraph subHeader15 = new Paragraph(listaP.Rangos, new Font(Font.FontFamily.HELVETICA, 7));
                subHeader15.Alignment = Element.ALIGN_CENTER;
                pdf.Add(subHeader15);
                Paragraph subHeader16 = new Paragraph(listaP.Cedula, new Font(Font.FontFamily.HELVETICA, 7));
                subHeader16.Alignment = Element.ALIGN_CENTER;
                pdf.Add(subHeader16);
                Paragraph subHeader17 = new Paragraph(listaP.desc_departamento + " TEL" + listaP.num_celular, new Font(Font.FontFamily.HELVETICA, 7));
                subHeader17.Alignment = Element.ALIGN_LEFT;
                pdf.Add(subHeader17);
                Paragraph subHeader18 = new Paragraph("YAN LIROY VARGAS CAMINERO", new Font(Font.FontFamily.HELVETICA, 7, Font.BOLD));
                subHeader18.SpacingBefore = 25f;
                subHeader18.Alignment = Element.ALIGN_CENTER;
                pdf.Add(subHeader18);
                Paragraph subHeader19 = new Paragraph("CAPITÁN DE NAVÍO", new Font(Font.FontFamily.HELVETICA, 7));
                subHeader19.Alignment = Element.ALIGN_CENTER;
                pdf.Add(subHeader19);
                Paragraph subHeader20 = new Paragraph("INTENDENTE DEL MAT.BELICO, ARD", new Font(Font.FontFamily.HELVETICA, 7));
                subHeader20.Alignment = Element.ALIGN_CENTER;
                pdf.Add(subHeader20);

                pdf.Close();
                writer.Close();


                byte[] content = ms.ToArray();
                var fileName = Guid.NewGuid().ToString() + "form25" + Path.GetExtension(".pdf");
                var imagePath = Path.Combine("wwwroot", "pdf", fileName);

                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    fileStream.Write(content, 0, content.Length);


                }

                return View(new FileStreamResult(ms, fileName));
            }
               // return View("CertificationS");
        }


        public async Task<IActionResult> ExportPDf()
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings =
        {
            ColorMode = ColorMode.Color,
            Orientation = Orientation.Portrait,
            PaperSize = PaperKind.A4,
        },
                Objects = {
            new ObjectSettings()
            {
                PagesCount = true,
                HtmlContent = "<h1>Hello World</h1>",
                WebSettings = { DefaultEncoding = "utf-8" },
                HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontSize = 9, Center = "Sample Footer" }
            }
        }
            };

            var pdfData = _converter.Convert(doc);
            return new FileContentResult(pdfData, "application/pdf");
        }

        [HttpPost]

        public IActionResult ReportIdCard(string documento)
        {
            documento = "'22400750893'";
            //// Asumiendo que _apiServiceAsignacion.GetVArmas() es asíncrono, deberías esperar el resultado adecuadamente.
            //// No uses .Result en una llamada asíncrona. Puede causar deadlocks y otros problemas.
            //// Idealmente, haz que tu método sea asíncrono y usa await.
            //var itemJetsky = _apiServiceAsignacion.GetVArmas().Result.Where(x => x.Asignacion_Documento == documento);

            //// Asegúrate de que tienes una ruta válida a tu archivo RDLC
            //var path = Path.Combine(_webHostEnvironment.WebRootPath, "Reporte", "BelicoReporteTenencia.rdlc");

            //// El diccionario de datos probablemente debería ser configurado de manera diferente
            //Dictionary<string, string> reportParameters = new Dictionary<string, string>
            //{
            //    { "Cedula", documento }
            //};

            //LocalReport localReport = new LocalReport(path);

            //// Configura los parámetros del reporte si son necesarios
            //localReport.AddDataSource("DataSet1", itemJetsky); // Asegúrate de que el nombre del DataSet coincida con el que está en tu archivo RDLC

            //// Renderiza el reporte a PDF
            //var result = localReport.Execute(RenderType.Pdf, parameters: reportParameters);

            //// Devuelve el archivo PDF
            // return File(result.MainStream, "application/pdf", "ReportIdCard.pdf");
            var itemJetsky = _apiServiceAsignacion.GetVArmas().Result.Where(x => x.Asignacion_Documento == documento);

            string mimType = "";
            int extension = 1;
            var path = $"{_webHostEnvironment.WebRootPath}\\Reporte\\BelicoReporteTenencia.rdlc";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("Cedula", documento);



            if (data.ContainsKey("Cedula"))
            {
                data["Cedula"] = null;
            }
            else
            {
#pragma warning disable S112 // General exceptions should never be thrown
                throw new Exception(String.Format("Key {0} was not found", "Cedula"));
#pragma warning restore S112 // General exceptions should never be thrown
            }
            LocalReport localReport = new LocalReport(path);
#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
            FieldInfo field = localReport.GetType().GetField("localReport", bindFlags);
            object rptObj = field.GetValue(localReport);
            Type type = rptObj.GetType();
            PropertyInfo pi = type.GetProperty("EnableExternalImages");
            pi.SetValue(rptObj, true, null);
            var result = localReport.Execute(RenderType.Pdf, extension, data, mimType);


            return File(result.MainStream, "Application/pdf");
        }
        public ActionResult xCert()
        {
            ViewBag.CertifierName = "YAN LIRIOY VARGAS CAMINERO";
            ViewBag.name = "prueba";
            ViewBag.CertifierTitle = "Capitán de Navio, ARD";
            ViewBag.CertificationDate = DateTime.Now.Day + " de " + DateTime.Now.ToString().AsDateTime().Month + " de " + DateTime.Now.Year;
            ViewBag.WeaponDetails = new List<string>
{
    "PISTOLA MARCA FM HI POWER CALIBRE 9MM NO. 404067",
    "CAPSULAS PISTOLA CALIBRE 9MM"
    // Add other weapon details
};
            return View();
        }  public ActionResult CertPos()

        {
            ViewBag.CertifierName = "YAN LIRIOY VARGAS CAMINERO";
            ViewBag.name = "prueba";
            ViewBag.CertifierTitle = "Capitán de Navio, ARD";
            ViewBag.CertificationDate = DateTime.Now.Day + " de " + DateTime.Now.ToString().AsDateTime().Month + " de " + DateTime.Now.Year;
            ViewBag.WeaponDetails = new List<string>
        {
            "PISTOLA MARCA FM HI POWER CALIBRE 9MM NO. 404067",
            "CAPSULAS PISTOLA CALIBRE 9MM"
            // Add other weapon details
        };
            return View();
        } 
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Crearpdf25(int selectedOption) 
        {
            VPersonal listaP = await _apiServiceAsignacion.GetVPersonaId(selectedOption);
            IEnumerable<AsignacionArma> Lasig = await _apiServiceAsignacion.GetAsignaciones();
            IEnumerable<VArma> LArma = await _apiServiceArma.GetVArmas();
            IEnumerable<VPertrecho> lPert = await _apiServiceAsignacion.GetVPertrechos();
            var datosPert = lPert.Where(x => x.Id_Militar == selectedOption && x.status).ToList();
            var listaArmaDto = new List<VArma>();
            var datos = Lasig.Where(x => x.AsignacionNoRango == selectedOption).ToList();
            var listaDto = new List<AsignacionArma>();
            List<VArma> listafilter = new List<VArma>();


            foreach (var asignacion in datos)
            {
                listaDto.Add(asignacion);

                foreach (var arma in LArma.Where(x=> x.IdArma == asignacion.IdArma)) {
                    listaArmaDto.Add(arma);
                     listafilter = listaArmaDto
                        .Where(x => x.IdArma == arma.IdArma)
                    .GroupBy(x => x.IdArma)  // Agrupar por IdArma para manejar duplicados
               .Select(g => g.First())  // Seleccionar el primer elemento de cada grupo
               .ToList();
                }

                

            }
            
            using (MemoryStream ms = new MemoryStream())
            {
               
                Rectangle rect = new Rectangle(150,200);
                rect.BorderColor= BaseColor.BLACK;
                rect.Border =Rectangle.BOX;
                rect.BorderWidth = 2;
                Document pdf = new Document(PageSize.B7);

                PdfWriter writer = PdfWriter.GetInstance(pdf, ms);


                pdf.AddAuthor("Armada");
                pdf.AddCreator("BelisoSys");
                pdf.AddSubject("ManejoBelico");
                pdf.AddTitle("BelisoSys");

                pdf.Open();
                Paragraph subHeader1 = new Paragraph("Form. No 25, ARD", new Font(Font.FontFamily.HELVETICA, 7));
                subHeader1.Alignment = Element.ALIGN_LEFT;
                pdf.Add(subHeader1);

                Paragraph subHeader2 = new Paragraph("ARMADA DE REPÚBLICA DOMINICANA", new Font(Font.FontFamily.HELVETICA, 7, Font.BOLD));
                subHeader2.Alignment = Element.ALIGN_CENTER;
                pdf.Add(subHeader2);
                Paragraph subHeader3 = new Paragraph("RECIBO MEMORANDUM", new Font(Font.FontFamily.HELVETICA, 7));
                subHeader3.Alignment = Element.ALIGN_CENTER;
                pdf.Add(subHeader3);
                Paragraph subHeader4 = new Paragraph("Santo Domingo, Este.", new Font(Font.FontFamily.HELVETICA, 7));
                subHeader4.Alignment = Element.ALIGN_CENTER;
                pdf.Add(subHeader4);
                Paragraph subHeader5 = new Paragraph("Lugar", new Font(Font.FontFamily.HELVETICA, 7));
                subHeader5.Alignment = Element.ALIGN_CENTER;
                pdf.Add(subHeader5);
                Paragraph subHeader6 = new Paragraph("--------------------------------------------------------------------", new Font(Font.FontFamily.HELVETICA, 7));
                subHeader6.Alignment = Element.ALIGN_LEFT;
                pdf.Add(subHeader6);
                Paragraph subHeader7 = new Paragraph(DateTime.Now.ToString("d MMMM yyyy", new System.Globalization.CultureInfo("es-ES")), new Font(Font.FontFamily.HELVETICA, 7));
                subHeader7.Alignment = Element.ALIGN_CENTER;
                pdf.Add(subHeader7);
                Paragraph subHeader8 = new Paragraph("Fecha", new Font(Font.FontFamily.HELVETICA, 7));
                subHeader8.Alignment = Element.ALIGN_CENTER;
                pdf.Add(subHeader8);
                pdf.Add(subHeader6);
                Paragraph subHeader9 = new Paragraph("ACUSO RECIBO AL, INTENDENTE DEL MAT.BELICO, ARD", new Font(Font.FontFamily.HELVETICA, 7));
                subHeader9.Alignment = Element.ALIGN_LEFT;
                pdf.Add(subHeader9);
                pdf.Add(subHeader6);
                Paragraph subHeader10 = new Paragraph("De los efectos siguientes para uso oficial del " + listaP.Rangos + listaP.Nombres, new Font(Font.FontFamily.HELVETICA, 7));
                subHeader10.Alignment = Element.ALIGN_LEFT;
                pdf.Add(subHeader10);
                Paragraph subHeader11 = new Paragraph("propiedades de la Armada de la ARD y me comprometo a devolverlos cuando sean requeridos por la autoridad competente.", new Font(Font.FontFamily.HELVETICA, 7));
                subHeader11.Alignment = Element.ALIGN_LEFT;
                pdf.Add(subHeader11);
                pdf.Add(subHeader6);
                Paragraph subHeader12 = new Paragraph("CANTIDAD    ARTICULO    VALOR", new Font(Font.FontFamily.HELVETICA, 7, Font.BOLD));
                subHeader12.Alignment = Element.ALIGN_LEFT;
                pdf.Add(subHeader12);
                pdf.Add(subHeader6);

                foreach (var item in listafilter) 
                {   Paragraph _par = new();

                    _par = new Paragraph("01.-"+item.TaNombre +" MARCA"+ item.ArmaMarcaDescripcion +" CAL."+ item.ArmaCalibre +" NO."+item.ArmaSerie, new Font(Font.FontFamily.HELVETICA, 7));
                    _par.Alignment = Element.ALIGN_LEFT;
                    pdf.Add(_par);

                }
                foreach (var item in datosPert)
                {
                    Paragraph _par = new();

                    _par = new Paragraph("0"+item.cantidad+ " " + item.pertrechos_descripcion , new Font(Font.FontFamily.HELVETICA, 7));
                    _par.Alignment = Element.ALIGN_LEFT;
                    pdf.Add(_par);

                }

                Paragraph subHeader13 = new Paragraph("-x-x-x-x-x-x-x-x-x-x-x-x--x-x-x-x-x-x-x-x-x-x-x-x-x-", new Font(Font.FontFamily.HELVETICA, 7));
                subHeader13.Alignment = Element.ALIGN_LEFT;
                pdf.Add(subHeader13); 
                Paragraph subHeader14 = new Paragraph(listaP.Nombres, new Font(Font.FontFamily.HELVETICA, 7, Font.BOLD));
                subHeader14.Alignment = Element.ALIGN_CENTER;
                subHeader14.SpacingBefore = 25f;
                pdf.Add(subHeader14);
                Paragraph subHeader15 = new Paragraph(listaP.Rangos, new Font(Font.FontFamily.HELVETICA, 7));
                subHeader15.Alignment = Element.ALIGN_CENTER;
                pdf.Add(subHeader15);
                Paragraph subHeader16 = new Paragraph(listaP.Cedula, new Font(Font.FontFamily.HELVETICA, 7));
                subHeader16.Alignment = Element.ALIGN_CENTER;
                pdf.Add(subHeader16); 
                Paragraph subHeader17 = new Paragraph(listaP.desc_departamento+ " TEL"+listaP.num_celular, new Font(Font.FontFamily.HELVETICA, 7));
                subHeader17.Alignment = Element.ALIGN_LEFT;
                pdf.Add(subHeader17);
                Paragraph subHeader18 = new Paragraph("YAN LIROY VARGAS CAMINERO", new Font(Font.FontFamily.HELVETICA, 7, Font.BOLD));
                subHeader18.SpacingBefore = 25f;
                subHeader18.Alignment = Element.ALIGN_CENTER;
                pdf.Add(subHeader18);
                Paragraph subHeader19 = new Paragraph("CAPITÁN DE NAVÍO", new Font(Font.FontFamily.HELVETICA, 7));
                subHeader19.Alignment = Element.ALIGN_CENTER;
                pdf.Add(subHeader19);
                Paragraph subHeader20 = new Paragraph("INTENDENTE DEL MAT.BELICO, ARD", new Font(Font.FontFamily.HELVETICA, 7));
                subHeader20.Alignment = Element.ALIGN_CENTER;
                pdf.Add(subHeader20);           

                pdf.Close();
                writer.Close();


                byte[] content = ms.ToArray();
                var fileName = Guid.NewGuid().ToString() +"form25" + Path.GetExtension(".pdf");
                var imagePath = Path.Combine("wwwroot", "pdf", fileName);

                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    fileStream.Write(content, 0, content.Length);
                  

                }

                return View(new FileStreamResult(ms, fileName));

                // Write out PDF from memory stream.
                //using (FileStream fs = File.Create("C:\\Test.pdf"))
                //{
                //    fs.Write(content, 0, (int)content.Length);
                //}

                //return null;
            }
        }


    }
}
