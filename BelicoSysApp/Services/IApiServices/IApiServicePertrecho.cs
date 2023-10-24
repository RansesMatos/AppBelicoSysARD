using BelicoSysApp.Models;

namespace BelicoSysApp.Services
{
    public interface IApiServicePertrecho
    {
        Task<ICollection<Pertrecho>> GetPertrecho();
        Task<ICollection<Almacen>> GetAlmacenes();
        Task<Pertrecho> Get(int IdPertrecho);

        Task<Pertrecho> Save(Pertrecho objeto);

        Task<bool> Edit(int idpertrecho,Pertrecho objeto);

        Task<bool> Delete(int idProducto);
    }
}
