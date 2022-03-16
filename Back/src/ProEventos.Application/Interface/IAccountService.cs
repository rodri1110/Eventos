
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProEventos.Application.DTOs;

namespace ProEventos.Application.Interface
{
    public interface IAccountService
    {
         Task<bool> UserExists(string userName);
         Task<UserUpdateDTO> GetUserByUserNameAsync(string userName);
         Task<SignInResult> CheckUserPasswordAsync(UserUpdateDTO userUpDateDTO, string password);
         Task<UserDTO> CreateAccountAsync(UserDTO userDTO);
         Task<UserUpdateDTO> UpdateAccount(UserUpdateDTO userUpdateDTO);
    }
}