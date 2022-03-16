using System.Collections.Generic;
using System.Threading.Tasks;
using ProEventos.Domain.Identity;

namespace ProEventos.Persistence.Repository.Interface
{
    public interface IUserRepository : IProEventosRepository
    {
         Task<IEnumerable<User>> GetUsersAsync();
         
         Task<User> GetUserByIdAsync(int Id);

         Task<User> GetUserByUserNameAsync(string userName);
    }
}