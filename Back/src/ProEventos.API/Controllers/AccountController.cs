using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interface;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;
        public AccountController(IAccountService accountService, 
                            ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser(){
            try
            {
                var userName = User.GetUserName();
                var user = await _accountService.GetUserByUserNameAsync(userName);
                return Ok(user);
            }
            catch (Exception ex)
            {
                 return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao verificar usuário. Erro: {ex.Message}");
            } 
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(UserDTO userDTO){
            try
            {
                if(await _accountService.UserExists(userDTO.UserName))
                return BadRequest("Nome de usuário já cadastrado");

                var user = await _accountService.CreateAccountAsync(userDTO);
                if(user != null) return Ok(new
                {
                    userName = user.UserName,
                    PrimeiroNome = user.PrimeiroNome,
                    token = _tokenService.CreateToken(user).Result
                });

                return BadRequest("Usuário não criado, tente novamente mais tarde!");
            }
            catch (Exception ex)
            {
                 return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao registrar usuário. Erro: {ex.Message}");
            } 
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDTO userLogin){
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(userLogin.Username);
                if(user == null) return Unauthorized("Usuário ou senha incorretos.");
                
                var result = await _accountService.CheckUserPasswordAsync(user, userLogin.Password);
                if(!result.Succeeded) return Unauthorized("Usuário ou senha incorretos.");
                
                return Ok(new
                {
                    userName = user.UserName,
                    PrimeiroNome = user.PrimeiroNome,
                    token = _tokenService.CreateToken(user).Result
                });
            }
            catch (Exception ex)
            {
                 return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao realizar login. Erro: {ex.Message}");
            } 
        }
        
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserUpdateDTO userUpdateDTO){
            try
            {
                if(userUpdateDTO.UserName != User.GetUserName()) 
                return Unauthorized("Usuário inválido!");

                var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());
                if(user == null) return Unauthorized("Usuário inválido.");

                var userReturn = await _accountService.UpdateAccount(userUpdateDTO);
                if(userReturn == null) return  NoContent();

                return Ok
                (
                    new 
                    {
                        userName = userReturn.UserName,
                        PrimeiroNome = userReturn.PrimeiroNome,
                        token = _tokenService.CreateToken(userReturn).Result
                    }
                );
            }
            catch (Exception ex)
            {
                 return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao atualizar usuário. Erro: {ex.Message}");
            } 
        }
    }
}