namespace BelicoSysApp.Models
{
    public class VPertrecho
    {
        public int Id_Asignacion_pertrecho { get; set; }

        public int Id_pertrechos { get; set; }

        public int Id_Militar { get; set; }
        public int cantidad { get; set; }
        public string pertrechos_descripcion { get; set; } = null!;
        public bool status { get; set; }

    }
}
