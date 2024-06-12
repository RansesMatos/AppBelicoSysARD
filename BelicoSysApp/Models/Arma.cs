namespace BelicoSysApp.Models
{
    public class Arma
    {
        public int? idArma { get; set; }
        public int? idTipoArma { get; set; }
        public int? IdArmaMarca { get; set; }
        public string armaCalibre { get; set; }
        public int? idModelo { get; set; }
        public string armaSerie { get; set; }
        public int? idAlmacen { get; set; }
        public bool? armaEstado { get; set; }
        public bool? armaStatus { get; set; }
        public bool? armaAsig { get; set; }
        public string ImagePath1 { get; set; }
        public string ImagePath2 { get; set; }
        public string ImagePath3 { get; set; }
        public string ImagePath4 { get; set; }
        public string? BarcodePath { get; set; } = null!;
        public string? ArmaNota { get; set; } = null!;
        public virtual ArmaMarca IdArmaMarcaNavigation { get; set; } = null!;
        public virtual TipoArma IdTipoArmaNavigation { get; set; } = null!;
        public virtual ArmaModelo IdTipoModeloNavigation { get; set; } = null!;
        public virtual Almacen IdAlmacenNavigation { get; set; } = null!;
    }


}


