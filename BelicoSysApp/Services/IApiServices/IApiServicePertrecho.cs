using BelicoSysApp.Models;

namespace BelicoSysApp.Services
{
    public interface IApiServicePertrecho
    {
        Task<ICollection<Pertrecho>> GetPertrecho();
        Task<Pertrecho> Get(int IdPertrecho);

        Task<Pertrecho> Save(Pertrecho objeto);

        Task<bool> Edit(Pertrecho objeto);

        Task<bool> Delete(int idProducto);
    }
}
