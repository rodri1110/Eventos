using System.Collections.Generic;
using System.Threading.Tasks;
using ProEventos.Application.DTOs;

namespace ProEventos.Application.Interface
{
    public interface IEventosService
    {
        Task<EventoDTO> AddEvento(EventoDTO model);
        Task<EventoDTO> UpDateEvento(int eventoId, EventoDTO model);
        Task<bool> DeleteEvento(int eventoId);
        
        Task<List<EventoDTO>> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);
        Task<EventoDTO[]> GetAllEventosAsync(bool includePalestrantes = false);       
        Task<EventoDTO> GetEventoByIdAsync(int Eventoid, bool includePalestrantes = false);
    }
}