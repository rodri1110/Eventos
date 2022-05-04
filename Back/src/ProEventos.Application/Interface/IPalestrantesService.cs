using System.Threading.Tasks;
using ProEventos.Application.DTOs;
using ProEventos.Persistence.Models;

namespace ProEventos.Application.Interface
{
    public interface IPalestrantesService
    {
        Task<PalestranteDTO> AddPalestrantes(int userId, PalestranteAddDTO model);
        Task<PalestranteDTO> UpDatePalestrante(int userId, PalestranteUpdateDTO model);
        Task<PageList<PalestranteDTO>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false);       
        Task<PalestranteDTO> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false);
    }
}