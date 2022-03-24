using System.Threading.Tasks;
using ProEventos.Application.DTOs;

namespace ProEventos.Application.Interface
{
    public interface ITokenService
    {
         Task<string> CreateToken(UserUpdateDTO userUpdateDTO);
    }
}