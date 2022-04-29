using System.Collections.Generic;
using System.Threading.Tasks;
using ProEventos.Domain;
using ProEventos.Persistence.Models;

namespace ProEventos.Persistence.Interface
{
    public interface IEventosRepository
    {
         Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes);
         
         Task<Evento> GetEventoByIdAsync(int userId, int Eventoid, bool includePalestrantes);
    }
}