namespace BelicoSysApp.Models
{
    public class Pertrecho
    {
        public int IdPertrechos { get; set; }

        public string PertrechosDescripcion { get; set; } = null!;

        public int Cantidad { get; set; }

        public int? IdAlmacen { get; set; }

        public List<Pertrecho> ListPertrecho { get; set; }
    }
}
