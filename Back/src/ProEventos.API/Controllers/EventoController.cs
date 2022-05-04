using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Interface;
using Microsoft.AspNetCore.Http;
using ProEventos.Application.DTOs;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using ProEventos.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using ProEventos.Persistence.Models;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly IEventosService _service;
        private readonly IAccountService _accountService;
        private readonly IUtil _util;
        private readonly string _destino = "Images";
        public EventoController(IEventosService service,
                                IAccountService accountService,
                                IUtil util)
        {
            _util = util;
            _service = service;
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEventos([FromQuery]PageParams pageParams)
        {
            try
            {
                var eventos = await _service.GetAllEventosAsync(User.GetUserId(), pageParams, true);
                if (eventos == null) return NoContent();

                Response.AddPagination(eventos.CurrentPage, eventos.TotalCount,eventos.PageSize, eventos.TotalPages);

                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }

        [HttpGet("{eventoId}")]
        public async Task<IActionResult> GetById(int eventoId)
        {
            try
            {
                var evento = await _service.GetEventoByIdAsync(User.GetUserId(), eventoId, true);
                if (evento == null) return NoContent();
                return Ok(evento);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao retornar Evento. Erro: {ex.Message}");
            }
        }

        [HttpPost("upload-image/{eventoId}")]
        public async Task<IActionResult> UploadImage(int eventoId)
        {
            try
            {
                var evento = await _service.GetEventoByIdAsync(User.GetUserId(), eventoId, true);
                if (evento == null) return NoContent();

                var file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    _util.DeleteImage(evento.ImagemURL, _destino);
                    evento.ImagemURL = await _util.SaveImage(file, _destino);
                }

                var eventoRetorno = await _service.UpDateEvento(User.GetUserId(), eventoId, evento);

                return Ok(eventoRetorno);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao atualizar foto do Evento. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDTO model)
        {
            try
            {
                var evento = await _service.AddEvento(User.GetUserId(), model);
                if (evento == null) return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao retornar Evento. Erro: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventoDTO model)
        {
            try
            {
                var evento = await _service.UpDateEvento(User.GetUserId(), id, model);
                if (evento == null) return NoContent();
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
                var evento = await _service.GetEventoByIdAsync(User.GetUserId(), id, true);
                if (evento == null) return NoContent();

                if (await _service.DeleteEvento(User.GetUserId(), id))
                {
                    _util.DeleteImage(evento.ImagemURL, _destino);
                    return Ok(new { message = "Evento Deletado" });
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao deletar evento. Erro: {ex.Message}");
            }
        }
    }
}
