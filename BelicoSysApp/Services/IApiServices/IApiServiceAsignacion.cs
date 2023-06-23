using BelicoSysApp.Models;

namespace BelicoSysApp.Services
{
    public interface IApiServiceAsignacion
    {
        Task<ICollection<AsignacionArma>> GetAsignaciones();
        Task<ICollection<VArma>> GetVArmas();
        Task<IEnumerable<VPersonal>> GetVPersonal(String nombre, String Status);
        Task<VPersonal> GetVPersonaId(decimal documentId);

        Task<ICollection<AsignarEstado>> GetAsigEstado();

        Task<AsignacionArma> Get(int idAsig);

        Task<AsignacionArma> Save(AsignacionArma objeto);

        Task<bool> Edit(AsignacionArma objeto);

        Task<bool> Delete(int idProducto);
    }
}
