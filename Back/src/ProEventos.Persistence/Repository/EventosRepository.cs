using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexto;
using ProEventos.Persistence.Interface;

namespace ProEventos.Persistence.Repository
{
    public class EventosRepository : IEventosRepository
    {
        private readonly ProEventosContext _context;

        public EventosRepository(ProEventosContext context)
        {
            _context = context;
        }

        public async Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestrantes = false)
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
            .Where(evento => evento.UserId == userId)
            .OrderBy(evento => evento.EventoId);
            return await query.ToArrayAsync();
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

        public async Task<List<Evento>> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false)
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
                    .OrderBy(evento => evento.Tema)
                    .Where(evento => evento.Tema.ToLower().Contains(tema.ToLower()) 
                        && evento.UserId == userId);
            
            return await query.ToListAsync();        
        }        
    }
}