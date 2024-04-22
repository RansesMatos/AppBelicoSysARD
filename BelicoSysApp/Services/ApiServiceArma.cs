using BelicoSysApp.Models;
using DocumentFormat.OpenXml.EMMA;
using Newtonsoft.Json;
using System;
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
        public async Task<ICollection<Arma>> GetArmasMasiva(int cantidad,int cantipo)
        {
            ICollection<Arma> armaList = new List<Arma>();
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            var response = await client.GetAsync($"/Arma/ArmasMasiva?cantidad={cantidad}&cantipo={cantipo}");
            if (response != null && response.IsSuccessStatusCode)
            {

                var json_respuesta = await response.Content.ReadAsStringAsync();
                ICollection<Arma> resultado = JsonConvert.DeserializeObject<ICollection<Arma>>(json_respuesta);
                armaList = resultado;
            }

            return armaList;

        }
        public async Task<VArma> GetVArmaSerial(string armaSerial)
        {
            VArma vArmaList = new VArma();
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            var response = await client.GetAsync($"api/VArmas/{armaSerial}");
            if (response != null && response.IsSuccessStatusCode)
            {

                var json_respuesta = await response.Content.ReadAsStringAsync();
                VArma resultado = JsonConvert.DeserializeObject<VArma>(json_respuesta);
                vArmaList = resultado;
            }

            return vArmaList;

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
        public async Task<ICollection<VArma>> GetVArmasF()
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
        public async Task<bool> Edit(ArmaUpdateDto objeto)
        {
           
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            string jsonString =JsonConvert.SerializeObject(objeto);
            Console.WriteLine("JSON Body:");
            Console.WriteLine(jsonString);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            
            var response = await client.PatchAsync($"/api/Arma/{objeto.idArma}", content);

            //var json_respuesta = await response.Content.ReadAsStringAsync();
            //var resultado = JsonConvert.DeserializeObject<ArmaUpdateDto>(json_respuesta);
            //ArmaUpdateDto armaUpdate = resultado;
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {

                return response.IsSuccessStatusCode;
            }

            return false;
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

        public async Task<bool> Delete(int idProducto)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            var response = await client.DeleteAsync($"api/Arma{idProducto}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;

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

        public async Task<ICollection<ArmaMarca>> GetArmasMarcas()
        {
            ICollection<ArmaMarca> armaMarcaList = new List<ArmaMarca>();
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            var response = await client.GetAsync("api/ArmaMarca");
            if (response != null && response.IsSuccessStatusCode)
            {

                var json_respuesta = await response.Content.ReadAsStringAsync();
                ICollection<ArmaMarca> resultado = JsonConvert.DeserializeObject<ICollection<ArmaMarca>>(json_respuesta);
                armaMarcaList = resultado;
            }

            return armaMarcaList;
        }
        public async Task<ICollection<ArmaModelo>> GetArmasModelo()
        {
            ICollection<ArmaModelo> armaModeloList = new List<ArmaModelo>();
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            var response = await client.GetAsync("api/ArmaModelo");
            if (response != null && response.IsSuccessStatusCode)
            {

                var json_respuesta = await response.Content.ReadAsStringAsync();
                ICollection<ArmaModelo> resultado = JsonConvert.DeserializeObject<ICollection<ArmaModelo>>(json_respuesta);
                armaModeloList = resultado;
            }

            return armaModeloList;
        }

        public async Task<ICollection<TipoArma>> GetArmasTipos()
        {
            ICollection<TipoArma> armaTipoList = new List<TipoArma>();
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            var response = await client.GetAsync("api/ArmaTipo");
            if (response != null && response.IsSuccessStatusCode)
            {

                var json_respuesta = await response.Content.ReadAsStringAsync();
                ICollection<TipoArma> resultado = JsonConvert.DeserializeObject<ICollection<TipoArma>>(json_respuesta);
                armaTipoList = resultado;
            }

            return armaTipoList;
        }
    }
}
