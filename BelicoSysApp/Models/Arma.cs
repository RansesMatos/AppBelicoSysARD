namespace BelicoSysApp.Models
{
    public class Arma
    {
        public int IdArma { get; set; }

        public int IdTipoArma { get; set; }

        public int IdArmaMarca { get; set; }

        public string ArmaCalibre { get; set; } = null!;

        public string ArmaSerie { get; set; } = null!;

        public int IdAlmacen { get; set; }

        public int ArmaEstado { get; set; }

        public bool ArmaStatus { get; set; }
        public string ImagePath { get; set; }

        public List<Arma>? ListArma { get; set; }
    }
}

