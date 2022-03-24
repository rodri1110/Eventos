using System.Collections.Generic;
using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Interface
{
    public interface IEventosRepository
    {
         Task<List<Evento>> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes);

         Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestrantes);
         
         Task<Evento> GetEventoByIdAsync(int userId, int Eventoid, bool includePalestrantes);
    }
}