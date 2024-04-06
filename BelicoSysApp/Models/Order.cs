namespace BelicoSysApp.Models
{
    public class Order

    {
        public int Orden_id { get; set; }
        public string? Orden_titulo { get; set; }
        public int Militar_ID { get; set; }
        public DateTime Orden_fecha { get; set; }
        public int Orden_total { get; set; }
        public char Orden_status { get; set; }
        public int Usuario_ID { get; set; }
    }
}
