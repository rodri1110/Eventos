using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interface;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Repository.Interface;

namespace ProEventos.Application.Service
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public AccountService(UserManager<User> userManager
                            ,SignInManager<User> signInManager
                            ,IMapper mapper
                            ,IUserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDTO userUpDateDTO, string password)
        {
            try
            {
                var user = await _userManager.Users
                .SingleOrDefaultAsync(user => user.UserName == userUpDateDTO.UserName.ToLower());

                return await _signInManager.CheckPasswordSignInAsync(user, password, false);
                // Se alterarmos para true o usuário é bloqueado após tentativa errada.
            }
            catch (Exception ex)
            {
                 throw new Exception($"Erro ao verificar password. Erro: {ex.Message}");
            }
        }

        public async Task<UserUpdateDTO> CreateAccountAsync(UserDTO userDTO)
        {
            try
            {
                var user = _mapper.Map<User>(userDTO);
                var result = await _userManager.CreateAsync(user, userDTO.Password);

                if(result.Succeeded){
                    var userToReturn = _mapper.Map<UserUpdateDTO>(user);
                    return userToReturn;
                }
                return null;
            }
            catch (Exception ex)
            {
                 throw new Exception($"Erro ao criar usuário. Erro: {ex.Message}");
            }
        }

        public async Task<UserUpdateDTO> GetUserByUserNameAsync(string userName)
        {
            try
            {
                var user = await _userRepository.GetUserByUserNameAsync(userName);
                if(user == null) return null;

                var userUpDateDTO = _mapper.Map<UserUpdateDTO>(user);
                return userUpDateDTO;
            }
            catch (Exception ex)
            {
                 throw new Exception($"Erro ao recuperar usuário. Erro: {ex.Message}");
            }
        }

        public async Task<UserUpdateDTO> UpdateAccount(UserUpdateDTO userUpdateDTO)
        {
            try
            {
                var user = await _userRepository.GetUserByUserNameAsync(userUpdateDTO.UserName);
                if(user == null) return null;

                userUpdateDTO.Id = user.Id;

                _mapper.Map(userUpdateDTO, user);

                if(userUpdateDTO.Password != null){
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, userUpdateDTO.Password);
                }

                _userRepository.UpDate<User>(user);

                if (await _userRepository.SaveChangesAsync())
                {
                    var userToReturn = await _userRepository.GetUserByUserNameAsync(user.UserName);
                    return _mapper.Map<UserUpdateDTO>(userToReturn);
                }
                return null;
            }
            catch (Exception ex)
            {
                 throw new Exception($"Erro ao atualizar usuário. Erro: {ex.Message}");
            }
        }

        public async Task<bool> UserExists(string userName)
        {
            try
            {
                return await _userManager.Users
                        .AnyAsync(user => user.UserName == userName.ToLower());
            }
            catch (Exception ex)
            {
                 throw new Exception($"Erro ao verificar se usuário existe. Erro: {ex.Message}");
            }
        }
    }
}