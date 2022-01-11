using System.Collections.Generic;
using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Interface
{
    public interface IEventosRepository
    {
         Task<List<Evento>> GetAllEventosByTemaAsync(string tema, bool includePalestrantes);

         Task<Evento[]> GetAllEventosAsync(bool includePalestrantes);
         
         Task<Evento> GetEventoByIdAsync(int Eventoid, bool includePalestrantes);
    }
}