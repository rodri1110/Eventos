using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interface;
using ProEventos.Persistence.Models;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PalestrantesController : ControllerBase
    {
        private readonly IPalestrantesService _service;
        private readonly IAccountService _account;
        private readonly IWebHostEnvironment _environment;
        public PalestrantesController(IPalestrantesService service, 
                                      IWebHostEnvironment hostEnvironment,
                                      IAccountService accountService)
        {
            _account = accountService;
            _service = service;
            _environment = hostEnvironment;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery]PageParams pageParams){
            
            try
            {
                var palestrantes = await _service.GetAllPalestrantesAsync(pageParams, true);
                if (palestrantes == null) return NoContent();

                Response.AddPagination(palestrantes.CurrentPage, palestrantes.TotalCount,palestrantes.PageSize, palestrantes.TotalPages);

                return Ok(palestrantes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar palestrantes. Erro: {ex.Message}");
            }
        }

        [HttpGet()]
        public async Task<IActionResult> GetPalestrante(){
            try
            {
                var palestrante = await _service.GetPalestranteByUserIdAsync(User.GetUserId(), true);
                if(palestrante == null) return NoContent();

                return Ok(palestrante);
                
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar palestrante. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(PalestranteAddDTO model){
            try
            {
                var palestrante = await _service.GetPalestranteByUserIdAsync(User.GetUserId(), false);
                if(palestrante == null){
                    palestrante = await _service.AddPalestrantes(User.GetUserId(), model);
                }                    
                    return Ok(palestrante);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao adicionar palestrante. Erro: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpDatePalestrante(PalestranteUpdateDTO model){
            try
            {
                var palestrante = await _service.UpDatePalestrante(User.GetUserId(), model);
                if(palestrante == null) return NoContent();

                return Ok(palestrante);
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao atualizar palestrante. Erro: {ex.Message}");
            }
        }
    }
}
