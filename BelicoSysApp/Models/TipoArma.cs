
namespace BelicoSysApp.Models;

public partial class TipoArma
{
    public int IdTipoArma { get; set; }

    public string? TaNombre { get; set; }

    public virtual ICollection<Arma> Armas { get; set; } = new List<Arma>();
}
