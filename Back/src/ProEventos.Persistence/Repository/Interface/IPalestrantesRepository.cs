using System.Threading.Tasks;
using ProEventos.Domain;
using ProEventos.Persistence.Models;
using ProEventos.Persistence.Repository.Interface;

namespace ProEventos.Persistence.Interface
{
    public interface IPalestrantesRepository : IProEventosRepository
    {
         Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false);
         Task<Palestrante> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false);
    }
}