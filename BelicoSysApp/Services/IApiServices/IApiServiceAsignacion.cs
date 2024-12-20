using BelicoSysApp.Models;

namespace BelicoSysApp.Services
{
    public interface IApiServiceAsignacion
    {
        Task<ICollection<AsignacionArma>> GetAsignaciones();
        Task<AsignacionPertrecho> UpdateAsignacionPertrecho(AsignacionPertrecho pertrecho);
        Task<ICollection<VPertrecho>> GetVPertrechos();
        Task<ICollection<AsignacionPertrecho>> GetAsignacionesPertrecho();
        Task<ICollection<VArma>> GetVArmas();
        Task<VArma> GetVArmaSerial(string armaSerial);
        Task<ICollection<VArma>> GetVArmasF();
        Task<ICollection<Pertrecho>> Getpertrechos();
        Task<IEnumerable<VPersonal>> GetVPersonal(String? carnet, String? cedula ,String status);
        Task<VPersonal> GetVPersonaId(decimal documentId);
        Task<Order> GetOrderIndi(int idorder);
        Task<AsignacionPertrecho> GetAsigPertrecho(int idAsigPertrecho);
        Task<AsignacionPertrecho> GetAsigPertrecho(int idAsigPertrecho, int idMilitar);
        Task<AsignacionPertrecho> ActualizarPertrecho(Pertrecho pertrecho);
        Task<ICollection<AsignarEstado>> GetAsigEstado();
        Task<AsignacionArma> Get(int idAsig);
        Task<OrderDetalle> GetODetalle(int idAsig);
        Task<ICollection<Order>> GetOrders();
        Task<int> GetOrderLastNumber();
        Task<AsignacionArma> Save(AsignacionArma objeto);
        Task<Order> SaveOrder(Order objeto);
        Task<AsignacionPertrecho> SaveAsigPertrecho(AsignacionPertrecho objeto);
        Task<OrderDetalle> SaveOrderDetalle(OrderDetalle objeto);
        Task<bool> ExisteFusil(string serie, int orderId);

        Task<bool> Edit(AsignacionArma objeto);

        Task<bool> Delete(int idProducto);
        Task<bool> DeleteAsignacionPertrecho(int idAsigPertercho);



    }
}
