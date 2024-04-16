namespace BelicoSysApp.Models
{
    public class Arma
    {
        //public int IdArma { get; set; }
        //public int IdTipoArma { get; set; }
        //public int IdArmaMarca { get; set; }
        //public int IdModelo { get; set; }
        //public string ArmaCalibre { get; set; } = null!;
        //public string ArmaSerie { get; set; } = null!;
        //public int IdAlmacen { get; set; }
        //public int ArmaEstado { get; set; }
        //public bool ArmaStatus { get; set; }
        //public bool ArmaAsig { get; set; }
        //public string? ImagePath { get; set; }

        public int idArma { get; set; }
        public int idTipoArma { get; set; }
        public int IdArmaMarca { get; set; }
        public string armaCalibre { get; set; }
        public int idModelo { get; set; }
        public string armaSerie { get; set; }
        public int idAlmacen { get; set; }
        public int armaEstado { get; set; }
        public bool armaStatus { get; set; }
        public bool armaAsig { get; set; }
        public object imagePath { get; set; }
        public string? BarcodePath { get; set; } = null!;
        public string? ArmaNota { get; set; } = null!;
    }


}


