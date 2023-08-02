namespace BelicoSysApp.Models
{
    public class AsignacionArma
    {
        public int IdAsignacion { get; set; }

        public Decimal? AsignacionNoRango { get; set; }
        public string? AsignacionNombre { get; set; }
        public string? Asignacion_rango { get; set; }

        public string? AsignacionDocumento { get; set; }

        public int? IdArma { get; set; }

        public string? NotaAsignacion { get; set; }

        public int? AsignacionTipo { get; set; }

        public DateTime? FechaAsignacion { get; set; }

        public DateTime? FechaDevolucion { get; set; }

        public int? AsignacionEstado { get; set; }

        public bool AsignacionStatus { get; set; }
        public List<AsignacionArma> ListaAsig { get; set; }
    }
}
