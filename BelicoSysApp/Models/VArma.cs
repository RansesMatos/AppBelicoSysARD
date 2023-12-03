namespace BelicoSysApp.Models
{
    public class VArma
    {
        public int IdArma { get; set; }

        public string? TaNombre { get; set; }

        public string ArmaMarcaDescripcion { get; set; } = null!;

        public string ArmaCalibre { get; set; } = null!;

        public string ArmaSerie { get; set; } = null!;

        public string AlmacenDescripcion { get; set; } = null!;

        public string ArmaEstadoDescripcion { get; set; } = null!;

        public bool ArmaStatus { get; set; }
        public string Asignacion_nombre { get; set; } = null!;
        public string Asignacion_rango { get; set; } = null!;
        public string Asignacion_Documento { get; set; } = null!;
        public DateTime Fecha_Asignacion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int IdAsignacion { get; set; }

    }
}
