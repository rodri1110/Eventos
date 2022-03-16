using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Contexto;
using ProEventos.Persistence.Repository.Interface;

namespace ProEventos.Persistence.Repository
{
    public class UserRepository : ProEventosRepository, IUserRepository
    {
        private readonly ProEventosContext _context;

        public UserRepository(ProEventosContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync(); 
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users
            .FindAsync(id);
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users
            .SingleOrDefaultAsync(user => 
            user.UserName == userName.ToLower());
        }
    }
}