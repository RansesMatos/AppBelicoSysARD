using BelicoSysApp.Models;

namespace BelicoSysApp.Services
{
    public interface IApiServiceAsignacion
    {
        Task<ICollection<AsignacionArma>> GetAsignaciones();
        Task<ICollection<VArma>> GetVArmas();
        Task<VArma> GetVArmaSerial(string armaSerial);
        Task<ICollection<VArma>> GetVArmasF();

        Task<ICollection<Pertrecho>> Getpertrechos();
        Task<IEnumerable<VPersonal>> GetVPersonal(String? nombre,String? cedula ,String status);
        Task<VPersonal> GetVPersonaId(decimal documentId);

        Task<ICollection<AsignarEstado>> GetAsigEstado();

        Task<AsignacionArma> Get(int idAsig);

        Task<AsignacionArma> Save(AsignacionArma objeto);

        Task<bool> Edit(AsignacionArma objeto);

        Task<bool> Delete(int idProducto);
    }
}
