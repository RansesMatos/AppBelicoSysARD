namespace BelicoSysApp.Models
{
    public class VPersonal

    {
        public decimal MilitarNo { get; set; }
        public string Rangos { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Nombres { get; set; } = null!;
        public string Cedula { get; set; }
        public string TipoDocumento { get; set; } = null!;
        public string Estatus { get; set; }
        public string desc_estatus { get; set; } = null!;
        public decimal codidog_rango { get; set; } 
        public string abreviatura_rango { get; set; }
        public string desc_rango { get; set; } = null!;
        public decimal orden_rango { get; set; }
        public decimal codigo_especialidad { get; set; }
        public string desc_especialidad { get; set; } = null!;
        public string num_celular { get; set; } = null!;
        public string desc_departamento { get; set; } = null!;
        public string NumeroCarnet { get; set; } = null!;
    }
}
