using BelicoSysApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
