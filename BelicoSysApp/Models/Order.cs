namespace BelicoSysApp.Models
{
    public class Order

    {
        public int Orden_id { get; set; }
        public string? Motivo { get; set; }
        public string? Document_File { get; set; }
        public int MilitarNo { get; set; }
        public DateTime Orden_fecha { get; set; }
        public int Orden_total { get; set; }
        public char Orden_status { get; set; }
        public int Usuario_ID { get; set; }
    }
}
