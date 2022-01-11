using System.Collections.Generic;
using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Application.Interface
{
    public interface IEventosService
    {
        Task<Evento> AddEvento(Evento model);
        Task<Evento> UpDateEvento(int eventoId, Evento model);
        Task<bool> DeleteEvento(int eventoId);
        
        Task<List<Evento>> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);
        Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false);       
        Task<Evento> GetEventoByIdAsync(int Eventoid, bool includePalestrantes = false);
    }
}