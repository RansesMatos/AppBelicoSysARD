using BelicoSysApp.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web.Razor.Text;
using System.Web.WebPages;

namespace BelicoSysApp.Controllers
{
    public class CertificationController : Controller
    {
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


        public async Task<IActionResult> ExportPDf(PdfBody cuerpoRepo)
        {
            //ICollection<VArma> lista = await _apiArma.GetVArmas();
            //var valuesList = lista.ToList();

            string fileName = "values.pdf";

            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

                document.Open();

                document.Add(cuerpoRepo);

                
                document.Close();

                return File(memoryStream.ToArray(), "application/pdf", fileName);
            }
        }
        public ActionResult xCert()
        {
            return View();
        }  public ActionResult CertPos()
        {
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
