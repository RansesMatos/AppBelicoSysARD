using BelicoSysApp.Models;
using Newtonsoft.Json;
using System.Text;

namespace BelicoSysApp.Services
{
    public class ApiServiceArma : IApiServiceArma
    {
        private static string _baseUrl;
        public ApiServiceArma()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            _baseUrl = builder.GetSection("ApiSetting:baseUrl").Value;
        }
        public async Task<ICollection<Arma>> GetArmas()
        {
            ICollection<Arma> armaList = new List<Arma>();
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            var response = await client.GetAsync("api/Arma");
            if(response != null && response.IsSuccessStatusCode) 
            {

                var json_respuesta = await response.Content.ReadAsStringAsync();
                ICollection<Arma> resultado = JsonConvert.DeserializeObject<ICollection<Arma>>(json_respuesta);
                armaList = resultado;
            }
           
            return armaList;

        }
        public async Task<ICollection<VArma>> GetVArmas()
        {
            ICollection<VArma> vArmaList = new List<VArma>();
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            var response = await client.GetAsync("api/VArmas");
            if (response != null && response.IsSuccessStatusCode)
            {

                var json_respuesta = await response.Content.ReadAsStringAsync();
                ICollection<VArma> resultado = JsonConvert.DeserializeObject<ICollection<VArma>>(json_respuesta);
                vArmaList = resultado;
            }

            return vArmaList;

        }
        public async Task<Arma> Get(int IdArma)
        {
            Arma objeto = new Arma();

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
          
            var response = await client.GetAsync($"api/Arma{IdArma}");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ApiResult>(json_respuesta);
                objeto = resultado.arma;
            }

            return objeto;
        
        }
        public Task<bool> Edit(Arma objeto)
        {
            throw new NotImplementedException();
        }
        public async Task<Arma> Save(Arma objeto)
        {
            Arma armacreada = new Arma();
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);

            var content = new StringContent(JsonConvert.SerializeObject(objeto), Encoding.UTF8, "application/json");            

            var response = await client.PostAsync("api/Arma", content);
            var json_respuesta = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<Arma>(json_respuesta);
            armacreada = resultado;          
            if (response.IsSuccessStatusCode)
            {
               
                return armacreada;
            }
            

            return null;
        }

        public Task<bool> Delete(int idProducto)
        {
            throw new NotImplementedException();
        }      
    }
}
