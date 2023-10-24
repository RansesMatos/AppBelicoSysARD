﻿using BelicoSysApp.Models;
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
        public async Task<ICollection<Pertrecho>> Getpertrechos()
        {
            ICollection<Pertrecho> pertrechoList = new List<Pertrecho>();
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            var response = await client.GetAsync("api/Pertrecho");
            if (response != null && response.IsSuccessStatusCode)
            {

                var json_respuesta = await response.Content.ReadAsStringAsync();
                ICollection<Pertrecho> resultado = JsonConvert.DeserializeObject<ICollection<Pertrecho>>(json_respuesta);
                pertrechoList = resultado;
            }

            return pertrechoList;

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

        public async Task<bool> Delete(int idProducto)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            var response = await client.DeleteAsync($"api/AsignarArma/{idProducto}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
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

        public async Task<IEnumerable<VPersonal>> GetVPersonal(string? nombre, string? cedula, string status)
        {
            IEnumerable<VPersonal> vArmaList = new List<VPersonal>();
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            if (nombre != null) 
            { 
                var response = await client.GetAsync($"/api/AsignarArma/Personal?nombre={nombre}&status={status}");
                if (response != null && response.IsSuccessStatusCode)
                {

                    var json_respuesta = await response.Content.ReadAsStringAsync();
                    ICollection<VPersonal> resultado = JsonConvert.DeserializeObject<ICollection<VPersonal>>(json_respuesta);
                    vArmaList = resultado;
                }

                return vArmaList;
            }
            else
            {
                var response = await client.GetAsync($"/api/AsignarArma/Personal?status={status}&cedula={cedula}");
                if (response != null && response.IsSuccessStatusCode)
                {

                    var json_respuesta = await response.Content.ReadAsStringAsync();
                    ICollection<VPersonal> resultado = JsonConvert.DeserializeObject<ICollection<VPersonal>>(json_respuesta);
                    vArmaList = resultado;
                }

                return vArmaList;
            }
           
        }

        public async Task<VPersonal> GetVPersonaId(decimal documentId)
        {
            VPersonal objeto = new VPersonal();

            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);

            var response = await client.GetAsync($"api/AsignarArma/PeronalInd/{documentId}");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<VPersonal>(json_respuesta);
                objeto = resultado;
            }

            return objeto; 
        }

      
    }
}