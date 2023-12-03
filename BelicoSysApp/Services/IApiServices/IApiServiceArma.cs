using BelicoSysApp.Models;

namespace BelicoSysApp.Services
{
    public interface IApiServiceArma
    {
        Task<ICollection<Arma>> GetArmas();
        Task<ICollection<VArma>> GetVArmas();
        Task<Arma> Get(int IdArma);
        Task<VArma> GetVArmaSerial(string armaSerial);

        Task<Arma> Save(Arma objeto);

        Task<bool> Edit(Arma objeto);

        Task<bool> Delete(int idProducto);
        Task<ICollection<Almacen>> GetAlmacenes();
        Task<ICollection<ArmaMarca>> GetArmasMarcas();
        Task<ICollection<TipoArma>> GetArmasTipos();
        Task<ICollection<ArmaModelo>> GetArmasModelo();



    }
}
