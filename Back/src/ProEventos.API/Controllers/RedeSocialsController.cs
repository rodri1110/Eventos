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
    [Route("api/[controller]")]
    public class RedeSocialsController : ControllerBase
    {
        public readonly IRedesSociaisService _redeSocialservice;
        public IEventosService _eventosService;
        public IPalestrantesService _palestrantesService;

        public RedeSocialsController(IRedesSociaisService redeSocialService, IEventosService eventosService, IPalestrantesService palestrantesService)
        {
            _eventosService = eventosService;
            _palestrantesService = palestrantesService;
            _redeSocialservice = redeSocialService;
        }

        [HttpGet("evento/{eventoId}")]
        public async Task<IActionResult> GetByEvento(int eventoId)
        {
            try
            {
                if (!(await AutorEvento(eventoId)))
                    return Unauthorized();

                var redesSociais = await _redeSocialservice.GetAllByEventoIdAsync(eventoId);
                if (redesSociais == null) return NoContent();

                return Ok(redesSociais);

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar Redes Sociais. Erro: {ex.Message}");
            }
        }

        [HttpGet("palestrante")]
        public async Task<IActionResult> GetByPalestrante()
        {
            try
            {
                var palestrante = await _palestrantesService.GetPalestranteByUserIdAsync(User.GetUserId(), false);
                if (palestrante == null) return Unauthorized();

                var redesSociais = await _redeSocialservice.GetAllByPalestranteIdAsync(palestrante.Id);//User.GetUserId()
                if (redesSociais == null) return NoContent();

                return Ok(redesSociais);

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar Redes Sociais. Erro: {ex.Message}");
            }
        }

        [HttpPut("evento/eventoId")]
        public async Task<IActionResult> SaveByEvento(int eventoId, RedeSocialDTO[] models)
        {
            try
            {
                if (!(await AutorEvento(eventoId)))
                    return Unauthorized();

                var redesSociais = await _redeSocialservice.SaveByEvento(eventoId, models);
                if (redesSociais == null) return NoContent();

                return Ok(redesSociais);

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao salvar Redes Sociais. Erro: {ex.Message}");
            }
        }

        [HttpPut("palestrante")]
        public async Task<IActionResult> SaveByPalestrante(RedeSocialDTO[] models)
        {
            try
            {
                var palestrante = await _palestrantesService.GetPalestranteByUserIdAsync(User.GetUserId(), false);
                if (palestrante == null) return Unauthorized();

                var redesSociais = await _redeSocialservice.SaveByPalestrante(palestrante.Id, models);
                if (redesSociais == null) return NoContent();

                return Ok(redesSociais);

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao salvar Redes Sociais. Erro: {ex.Message}");
            }
        }

        [HttpDelete("evento/{eventoId}/{redeSocialId}")]
        public async Task<IActionResult> DeleteByEvento(int eventoId, int redeSocialId)
        {
            try
            {
                if (!(await AutorEvento(eventoId)))
                    return Unauthorized();

                return await _redeSocialservice.DeleteByEvento(eventoId, redeSocialId) ? 
                    Ok (new { message = "Rede Social deletada com sucesso."}) : 
                    throw new Exception("Rede Social não pode ser deletada, consulte o administrador.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao deletar Redes Sociais. Erro: {ex.Message}");
            }
        }

        [HttpDelete("palestrante/{redeSocialId}")]
        public async Task<IActionResult> DeleteByPalestrante(int redeSocialId)
        {
            try
            {
                var palestrante = await _palestrantesService.GetPalestranteByUserIdAsync(User.GetUserId(), false);
                if (palestrante == null) return Unauthorized();

                return await _redeSocialservice.DeleteByPalestrante(palestrante.Id, redeSocialId) ? 
                    Ok (new { message = "Rede Social deletada com sucesso."}) : 
                    throw new Exception("Rede Social não pode ser deletada, consulte o administrador.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao deletar Redes Sociais. Erro: {ex.Message}");
            }
        }

        [NonAction]
        private async Task<bool> AutorEvento(int eventoId)
        {

            var evento = await _eventosService.GetEventoByIdAsync(User.GetUserId(), eventoId, false);
            if (evento == null) return false;

            return true;
        }
    }
}