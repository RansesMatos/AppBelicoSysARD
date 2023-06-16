using BelicoSysApp.Models;
using Newtonsoft.Json;
using System.Text;

namespace BelicoSysApp.Services
{
    public class ApiServiceAsingacion : IApiServiceAsignacion
    {
        private static string _baseUrl;
        public ApiServiceAsingacion()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            _baseUrl = builder.GetSection("ApiSetting:baseUrl").Value;
        }
        public async Task<ICollection<AsignacionArma>> GetAsignaciones()
        {
            ICollection<AsignacionArma> armaList = new List<AsignacionArma>();
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            var response = await client.GetAsync("api/AsignarArma");
            if(response != null && response.IsSuccessStatusCode) 
            {

                var json_respuesta = await response.Content.ReadAsStringAsync();
                ICollection<AsignacionArma> resultado = JsonConvert.DeserializeObject<ICollection<AsignacionArma>>(json_respuesta);
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
        public async Task<AsignacionArma> Get(int idAsig)
        {
            AsignacionArma objeto = new AsignacionArma();

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
          
            var response = await client.GetAsync($"api/AsignarArma{idAsig}");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<ApiResult>(json_respuesta);
                objeto = resultado.Asignacion;
            }

            return objeto;
        
        }
        public Task<bool> Edit(AsignacionArma objeto)
        {
            throw new NotImplementedException();
        }
        public async Task<AsignacionArma> Save(AsignacionArma objeto)
        {
            AsignacionArma asignacioncreada = new AsignacionArma();
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);

            var content = new StringContent(JsonConvert.SerializeObject(objeto), Encoding.UTF8, "application/json");            

            var response = await client.PostAsync("api/AsignarArma", content);
            var json_respuesta = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<AsignacionArma>(json_respuesta);
            asignacioncreada = resultado;          
            if (response.IsSuccessStatusCode)
            {
               
                return asignacioncreada;
            }
            

            return null;
        }

        public Task<bool> Delete(int idProducto)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<AsignarEstado>> GetAsigEstado()
        {
            ICollection<AsignarEstado> asigEstadoList = new List<AsignarEstado>();
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            var response = await client.GetAsync("api/AsignarArma/Estado");
            if (response != null && response.IsSuccessStatusCode)
            {

                var json_respuesta = await response.Content.ReadAsStringAsync();
                ICollection<AsignarEstado> resultado = JsonConvert.DeserializeObject<ICollection<AsignarEstado>>(json_respuesta);
                asigEstadoList = resultado;
            }

            return asigEstadoList;

        }
    }
}
