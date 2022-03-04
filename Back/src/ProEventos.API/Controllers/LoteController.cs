using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.Application.Interface;
using ProEventos.Domain;
using ProEventos.Persistence.Contexto;
using ProEventos.Application.DTOs;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoteController : ControllerBase
    {
        private readonly ILoteService _service;
        
        public LoteController(ILoteService service)
        {
            _service = service;
        }

        [HttpGet("{eventoId}")]
        public async Task<IActionResult> GetLotes(int eventoId)
        {
            try
            {
                var lotes = await _service.GetLotesByEventoIdAsync(eventoId);
                if(lotes == null) return NoContent();
                return Ok(lotes);
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar lotes. Erro: {ex.Message}");
            }
        }

        [HttpPut("{eventoId}")]
        public async Task<IActionResult> SaveLotes(int eventoId, LoteDTO[] models)
        {
            try
            {
                 var lotes = await _service.SaveLotes(eventoId, models);
                 if(lotes == null)return NoContent();
                 return Ok(lotes);
            }
            catch (Exception ex)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao salvar lotes. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{eventoId}/{loteId}")]
        public async Task<IActionResult> DeleteLote(int eventoId, int loteId)
        {
            try
            {
                var lote = await _service.GetLoteByIdsAsync(eventoId, loteId);
                if(lote == null) NoContent();

                return await _service.DeleteLote(lote.EventoId, lote.Id)
                ? Ok(new {message = "Lote Deletado"}) 
                : throw new Exception ("Erro específico ao deletar lote."); 
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao deletar lote. Erro: {ex.Message}");
            }
        }
    }
}
