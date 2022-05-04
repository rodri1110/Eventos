using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Repository.Interface
{
    public interface IRedeSocialRepository : IProEventosRepository
    {
         Task<RedeSocial>GetRedeSocialEventoByIdsAsync(int eventoId, int id);
         Task<RedeSocial>GetRedeSocialPalestranteByIdsAsync(int palestranteId, int id);
         Task<RedeSocial[]>GetAllByEventoIdAsync(int eventoId);
         Task<RedeSocial[]>GetAllByPalestranteIdAsync(int palestranteId);
    }
}