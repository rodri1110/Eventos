using System.Threading.Tasks;
using ProEventos.Application.DTOs;

namespace ProEventos.Application.Interface
{
    public interface IRedesSociaisService
    {
         Task<RedeSocialDTO[]> SaveByEvento(int eventoId, RedeSocialDTO[] models);
         
         Task<bool> DeleteByEvento(int eventoId, int RedeSocialId);

         Task<RedeSocialDTO[]> SaveByPalestrante(int palestranteid, RedeSocialDTO[] models);
         Task<bool> DeleteByPalestrante(int palestranteId, int RedeSocialId);

         Task<RedeSocialDTO[]> GetAllByEventoIdAsync(int eventoId);

         Task<RedeSocialDTO[]> GetAllByPalestranteIdAsync(int palestranteId);

         Task<RedeSocialDTO> GetRedeSocialEventoByIdsAsync(int eventoId, int RedeSocialId);

         Task<RedeSocialDTO> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int RedeSocialId);
    }
}