namespace BelicoSysApp.Models
{
    public class ApiResult
    {
        public List<Arma> ListArma { get; set; }
        public List<Pertrecho> ListPertrecho { get; set; }


        public Arma? arma { get; set; }

        public Pertrecho pertrecho { get; set; }

        public AsignacionArma Asignacion { get; set; }
        public List<AsignacionArma> ListaAsig { get; set; }
        public List<VArma> ListVArma { get; set; }  
        public VArma? VArma { get; set; }   
    }
}
