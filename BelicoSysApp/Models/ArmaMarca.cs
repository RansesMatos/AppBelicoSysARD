

namespace BelicoSysApp.Models;

public partial class ArmaMarca
{
    public int IdArmaMarca { get; set; }

    public string ArmaMarcaDescripcion { get; set; } = null!;

    public DateTime? FechaCreacion { get; set; }  

    public virtual ICollection<Arma> Armas { get; set; } = new List<Arma>();
}
