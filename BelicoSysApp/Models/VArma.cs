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

        public DateTime FechaRegistro { get; set; }

    }
}
