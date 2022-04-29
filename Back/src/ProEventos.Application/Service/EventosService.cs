using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interface;
using ProEventos.Domain;
using ProEventos.Persistence.Interface;
using ProEventos.Persistence.Models;
using ProEventos.Persistence.Repository.Interface;

namespace ProEventos.Application.Service
{
    public class EventosService : IEventosService
    {
        private readonly IEventosRepository _eventosRepository;
        private readonly IProEventosRepository _proEventosRepository;

        private readonly IMapper _mapper;
        public EventosService(IProEventosRepository proEventosRepository,
                              IEventosRepository eventosRepository, 
                              IMapper mapper)
        {
            _proEventosRepository = proEventosRepository;
            _eventosRepository = eventosRepository;
            _mapper = mapper;
        }
        public async Task<EventoDTO> AddEvento(int userId, EventoDTO model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);
                evento.UserId = userId;

                _proEventosRepository.Add<Evento>(evento);
                
                if(await _proEventosRepository.SaveChangesAsync())
                {
                    var eventoRetorno = await _eventosRepository.GetEventoByIdAsync(userId, evento.EventoId, false);
                    return _mapper.Map<EventoDTO>(eventoRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<EventoDTO> UpDateEvento(int userId, int eventoId, EventoDTO model)
        {
            try
            {
                var evento = await _eventosRepository.GetEventoByIdAsync(userId, eventoId, false);
                if(evento == null) return null;

                model.EventoId = eventoId;
                model.UserId = userId; 

                _mapper.Map(model, evento);

                _proEventosRepository.UpDate<Evento>(evento);

                if(await _proEventosRepository.SaveChangesAsync())
                {
                    var eventoRetorno = await _eventosRepository.GetEventoByIdAsync(userId, eventoId, false);
                    
                    return _mapper.Map<EventoDTO>(eventoRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int userId, int eventoId)
        {
            try
            {
                var evento = await _eventosRepository.GetEventoByIdAsync(userId, eventoId, false);

                _proEventosRepository.Delete<Evento>(evento);

                return await _proEventosRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageList<EventoDTO>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventosRepository.GetAllEventosAsync(userId, pageParams, includePalestrantes);
                if(eventos == null) throw new Exception ("Não há Eventos cadastrados.");

                var resultados = _mapper.Map<PageList<EventoDTO>>(eventos);

                resultados.CurrentPage = eventos.CurrentPage;
                resultados.TotalPages = eventos.TotalPages;
                resultados.PageSize = eventos.PageSize;
                resultados.TotalCount = eventos.TotalCount;

                return resultados;
            }
            catch (Exception ex)
            {                
                throw new Exception (ex.Message);
            }
        }

        public async Task<EventoDTO> GetEventoByIdAsync(int userId, int eventoid, bool includePalestrantes = false)
        {
            try
            {
                 var evento = await _eventosRepository.GetEventoByIdAsync(userId, eventoid, includePalestrantes);

                 if(evento == null) throw new Exception("Evento não encontrado.");

                 var resultado = _mapper.Map<EventoDTO>(evento);
                 
                 return resultado;
            }
            catch (Exception ex)
            {                
                throw new Exception (ex.Message);
            }
        }
    }
}