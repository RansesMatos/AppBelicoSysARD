using BelicoSysApp.Models;

namespace BelicoSysApp.Services
{
    public interface IApiServiceAsignacion
    {
        Task<ICollection<AsignacionArma>> GetAsignaciones();
        Task<ICollection<VArma>> GetVArmas();
        Task<ICollection<AsignarEstado>> GetAsigEstado();

        Task<AsignacionArma> Get(int idAsig);

        Task<AsignacionArma> Save(AsignacionArma objeto);

        Task<bool> Edit(AsignacionArma objeto);

        Task<bool> Delete(int idProducto);
    }
}
