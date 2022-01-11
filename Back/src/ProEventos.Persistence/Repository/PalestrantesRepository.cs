using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexto;

namespace ProEventos.Persistence.Repository
{
    public class PalestrantesRepository
    {
        private readonly ProEventosContext _context;

        public PalestrantesRepository(ProEventosContext context)
        {
            _context = context;
        }

        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(palestrante => palestrante.RededesSociais);
            
            if(includeEventos)
            {
                query = query
                    .Include(palestrante => palestrante.PalestrantesEventos)
                    .ThenInclude(palestranteEvento => palestranteEvento.Evento);
            }

            query = query.OrderBy(palestrante => palestrante.Nome);

            return await query.ToArrayAsync();
        }

        public async Task<List<Palestrante>> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(palestrante => palestrante.RededesSociais);
            
            if(includeEventos)
            {
                query = query
                .Include(palestrante => palestrante.PalestrantesEventos)
                .ThenInclude(palestranteEvento => palestranteEvento.Evento);
            }

            query = query            
            .OrderBy(palestrante => palestrante.Nome)
            .Where(palestrante => palestrante.Nome.ToLower()
            .Contains(nome.ToLower()));

            return await query.ToListAsync();
        }

        public async Task<Palestrante> GetPalestranteByIdAsync(int palestranteId, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(palestrante => palestrante.RededesSociais);

            if(includeEventos)
            {
                query = query
                    .Include(palestrante => palestrante.PalestrantesEventos)
                    .ThenInclude(palestranteEvento => palestranteEvento.Evento);
            }

            query = query.Where(palestrante => palestrante.Id == palestranteId);

            return await query.FirstOrDefaultAsync();
        }
    }
}