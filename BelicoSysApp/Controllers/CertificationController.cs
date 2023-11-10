using AspNetCore.Reporting;
using BelicoSysApp.Models;
using BelicoSysApp.Services;
using DinkToPdf;
using DinkToPdf.Contracts;

using Microsoft.AspNetCore.Mvc;

using Microsoft.Reporting;
using System.Reflection;

using System.Web.WebPages;


namespace BelicoSysApp.Controllers
{
    public class CertificationController : Controller
    {
        private readonly IApiServiceAsignacion _apiServiceAsignacion;
        private readonly IConverter _converter;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CertificationController(IConverter converter, IApiServiceAsignacion apiServiceAsignacion, IWebHostEnvironment webHostEnvironment)
        {
            _converter = converter;
            _apiServiceAsignacion = apiServiceAsignacion;
            _webHostEnvironment = webHostEnvironment;
        }
        // GET: CertificationController
        [HttpGet]
        public ActionResult CertificationS()
        {
            
            CertificationReport reportData = new CertificationReport();
            ViewBag.CertifierName = "YAN LIRIOY VARGAS CAMINERO";
            ViewBag.name = "prueba";
            ViewBag.CertifierTitle = "Capitán de Navio, ARD";
            ViewBag.CertificationDate = DateTime.Now.Day +" de " + DateTime.Now.ToString().AsDateTime().Month + " de " + DateTime.Now.Year;
            ViewBag.WeaponDetails = new List<string>
        {
            "PISTOLA MARCA FM HI POWER CALIBRE 9MM NO. 404067",
            "CAPSULAS PISTOLA CALIBRE 9MM"
            // Add other weapon details
        };


            return View("CertificationS", reportData);
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
        //public IActionResult GenerateReport()
        //{
        //    CertificationReport reportData = new CertificationReport
        //    {
        //        CertifierName = "YAN LIRIOY VARGAS CAMINERO",
        //        CertifierTitle = "Capitán de Navio, ARD",
        //        CertificationDate = "13 de marzo de 2023",
        //        WeaponDetails = new List<string>
        //{
        //    "PISTOLA MARCA FM HI POWER CALIBRE 9MM NO. 404067",
        //    "CAPSULAS PISTOLA CALIBRE 9MM"
        //    // Add other weapon details
        //}
        //    };

        //    return View("CertificationReport", reportData);
        //}


    }
}
