using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.API.Models;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        public EventoController()
        {
        }

        public IEnumerable<Evento> _evento = new Evento[]{
            new Evento(){
                EventoId = 1,
                Local = "Araraquara",
                DataEvento = DateTime.Now.AddDays(2).ToString(),
                Tema = "Angular 11",
                QtdPessoas = 20,
                Lote = "1º Lote",
                ImagemUrl = "foto.png"
            },
            new Evento(){
                EventoId = 2,
                Local = "Campinas",
                DataEvento = DateTime.Now.AddDays(1).ToString(),
                Tema = "DotNet",
                QtdPessoas = 40,
                Lote = "2º Lote",
                ImagemUrl = "Dotnet.png"
            }
        };

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
            return _evento;
        }

        [HttpGet("{id}")]
        public IEnumerable<Evento> GetById(int id)
        {
            return _evento.Where(x => x.EventoId == id);
        }

        [HttpPost]
        public string Post()
        {
            return "Exemplo de post";
        }

        [HttpPut("{id}")]
        public string Put(int id)
        {
            return $"Exemplo de put id = {id}";
        }

        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            return $"Exemplo de delete id = {id}";
        }

    }
}
