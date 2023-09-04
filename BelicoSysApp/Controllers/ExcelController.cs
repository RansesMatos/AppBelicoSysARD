using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BelicoSysApp.Controllers
{
    public class ExcelController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ExcelController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Export()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var apiResponse = await httpClient.GetAsync("https://localhost:7090/api/Arma");

            if (apiResponse.IsSuccessStatusCode)
            {
                var valuesList = await apiResponse.Content.ReadAsStringAsync();

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                    // Add headers
                    worksheet.Cells[1, 1].Value = "Index";
                    worksheet.Cells[1, 2].Value = "Value";

                    // Add data
                    for (int i = 0; i < valuesList.Length; i++)
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
    }
}
