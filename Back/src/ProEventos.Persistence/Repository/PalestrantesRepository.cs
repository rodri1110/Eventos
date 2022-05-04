using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexto;
using ProEventos.Persistence.Interface;
using ProEventos.Persistence.Models;

namespace ProEventos.Persistence.Repository
{
    public class PalestrantesRepository : ProEventosRepository, IPalestrantesRepository
    {
        private readonly ProEventosContext _context;

        public PalestrantesRepository(ProEventosContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PageList<Palestrante>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(p => p.User)
                .Include(p => p.RedesSociais);
            
            if(includeEventos)
            {
                query = query
                    .Include(p => p.PalestrantesEventos)
                    .ThenInclude(pe => pe.Evento);
            }

            query = query.AsNoTracking()
                         .Where(p => (p.MiniCurriculo.ToLower().Contains(pageParams.Termo.ToLower()) || 
                                      p.User.PrimeiroNome.ToLower().Contains(pageParams.Termo.ToLower()) || 
                                      p.User.UltimoNome.ToLower().Contains(pageParams.Termo.ToLower())) && 
                                      p.User.Funcao == Domain.Enum.Funcao.Palestrante)
                         .OrderBy(p => p.Id);

            return await PageList<Palestrante>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<Palestrante> GetPalestranteByUserIdAsync(int userId, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(palestrante => palestrante.User)
                .Include(palestrante => palestrante.RedesSociais);

            if(includeEventos)
            {
                query = query
                    .Include(palestrante => palestrante.PalestrantesEventos)
                    .ThenInclude(palestranteEvento => palestranteEvento.Evento);
            }

            query = query.AsNoTracking().OrderBy(p => p.Id)
                         .Where(p => p.UserId == userId);

            return await query.FirstOrDefaultAsync();
        }
    }
}