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

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly IEventosService _service;
        public EventoController(IEventosService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetEventos()
        {
            try
            {
                var eventos = await _service.GetAllEventosAsync(true);
                if(eventos == null) return NotFound ("Nenhum evento encontrado!");

                return Ok(eventos);
            }
            catch (Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                 var evento = await _service.GetEventoByIdAsync(id, true);
                 if(evento == null) return NotFound ("Nenhum evento encontrado!");
                 return Ok(evento);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao retornar Evento. Erro: {ex.Message}");
            }
        }

        [HttpGet("tema/{tema}")]
        public async Task<IActionResult> GetByTema(string tema)
        {
            try
            {
                 var evento = await _service.GetAllEventosByTemaAsync(tema, true);
                 if(evento == null) return NotFound ("Nenhum evento com este tema encontrado!");
                 return Ok(evento);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao retornar Evento. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Evento model)
        {
            try
            {
                 var evento = await _service.AddEvento(model);
                 if(evento == null) return BadRequest("Erro ao tentar adicionar evento!");
                 return Ok(evento);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao retornar Evento. Erro: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Evento model)
        {
            try
            {
                 var evento = await _service.UpDateEvento(id, model);
                 if(evento == null)return BadRequest("Erro ao atualizar evento");
                 return Ok(evento);
            }
            catch (Exception ex)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao atualizar Evento. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                return await _service.DeleteEvento(id) ? Ok("Evento Deletado") : BadRequest("Erro ao deletar evento."); 
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao deletar evento. Erro: {ex.Message}");
            }
        }
    }
}
