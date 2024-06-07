using BelicoSysApp.Models;
using Newtonsoft.Json;
using System.Text;

namespace BelicoSysApp.Services
{
    public class ApiServicePertrecho : IApiServicePertrecho
    {
        private static string _baseUrl;
        public ApiServicePertrecho()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            _baseUrl = builder.GetSection("ApiSetting:baseUrl").Value;
        }
        public async Task<ICollection<Pertrecho>> GetPertrecho()
        {
            ICollection<Pertrecho> PertrechoList = new List<Pertrecho>();
            HttpClientHandler clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            var client = new HttpClient(clientHandler);
            client.BaseAddress = new Uri(_baseUrl);
            var response = await client.GetAsync("api/Pertrecho");
            if(response != null && response.IsSuccessStatusCode) 
            {

                var json_respuesta = await response.Content.ReadAsStringAsync();
                ICollection<Pertrecho> resultado = JsonConvert.DeserializeObject<ICollection<Pertrecho>>(json_respuesta);
                PertrechoList = resultado;
            }

#pragma warning disable CS8603 // Possible null reference return.
            return PertrechoList;
#pragma warning restore CS8603 // Possible null reference return.

        }
        public async Task<Pertrecho> Get(int IdPertrecho)
        {
            Pertrecho objeto = new Pertrecho();

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
          
            var response = await client.GetAsync($"api/Pertrecho/{IdPertrecho}");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<Pertrecho>(json_respuesta);
                objeto = resultado;
            }
            return objeto;
        }
        public async Task<ICollection<Almacen>> GetAlmacenes()
        {
            ICollection<Almacen> almacenList = new List<Almacen>();
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            var response = await client.GetAsync("api/Almacen");
            if (response != null && response.IsSuccessStatusCode)
            {

                var json_respuesta = await response.Content.ReadAsStringAsync();
                ICollection<Almacen> resultado = JsonConvert.DeserializeObject<ICollection<Almacen>>(json_respuesta);
                almacenList = resultado;
            }

            return almacenList;
        }
        public async Task<bool> Edit(int idpertrecho, Pertrecho objeto)
        {
            _ = new Pertrecho();
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);

            var content = new StringContent(JsonConvert.SerializeObject(objeto), Encoding.UTF8, "application/json");

            var response = await client.PatchAsync($"api/Pertrecho/{idpertrecho}", content);
            var json_respuesta = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<Pertrecho>(json_respuesta);
            Pertrecho pertrcho = resultado;

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {

                return response.IsSuccessStatusCode;
            }

            return false;
        }
        public async Task<Pertrecho?> Save(Pertrecho objeto)
        {
            _ = new Pertrecho();
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);

            var content = new StringContent(JsonConvert.SerializeObject(objeto), Encoding.UTF8, "application/json");            

            var response = await client.PostAsync("api/Pertrecho", content);
            var json_respuesta = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<Pertrecho>(json_respuesta);
            Pertrecho pertrcho = resultado;
            if (response.IsSuccessStatusCode)
            {
               
                return pertrcho;
            }
            

            return null;
        }

        public Task<bool> Delete(int idProducto)
        {
            throw new NotImplementedException();
        }      
    }
}
