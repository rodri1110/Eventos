using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexto;
using ProEventos.Persistence.Interface;
using ProEventos.Persistence.Models;

namespace ProEventos.Persistence.Repository
{
    public class EventosRepository : IEventosRepository
    {
        private readonly ProEventosContext _context;

        public EventosRepository(ProEventosContext context)
        {
            _context = context;
        }

        public async Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(evento => evento.Lotes)
            .Include(evento => evento.RedesSociais);

            if(includePalestrantes)
            {
                query = query
                .Include(evento => evento.PalestrantesEventos)
                .ThenInclude(palestranteEvento => palestranteEvento.Palestrante);
            }

            query = query
            .AsNoTracking()
            .Where(evento => evento.Tema.ToLower().Contains(pageParams.Termo.ToLower()) 
            || evento.Local.ToLower().Contains(pageParams.Termo.ToLower()) 
            && evento.UserId == userId)
            .OrderBy(evento => evento.DataEvento);
            return await PageList<Evento>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<Evento> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos.AsNoTracking()
                .Include(evento => evento.Lotes)
                .Include(evento => evento.RedesSociais);
            
            if(includePalestrantes)
            {
                query = query
                    .Include(evento => evento.PalestrantesEventos)
                    .ThenInclude(palestranteEvento => palestranteEvento.Palestrante);
            }

            query = query.AsNoTracking()
                        .Where(evento => evento.EventoId == eventoId
                         &&  evento.UserId == userId);

            return await query.FirstOrDefaultAsync();
        }
    }
}