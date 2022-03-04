using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Interface
{
    public interface ILoteRepository
    {
         Task<Lote[]> GetLotesByEventoIdAsync(int eventoId);
         
         Task<Lote> GetLoteByIdsAsync(int eventoId, int LoteId);
    }
}