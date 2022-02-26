using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interface;
using ProEventos.Domain;
using ProEventos.Persistence.Interface;
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
        public async Task<EventoDTO> AddEvento(EventoDTO model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);

                _proEventosRepository.Add<Evento>(evento);
                
                if(await _proEventosRepository.SaveChangesAsync())
                {
                    var eventoRetorno = await _eventosRepository.GetEventoByIdAsync(evento.EventoId, false);
                    return _mapper.Map<EventoDTO>(eventoRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<EventoDTO> UpDateEvento(int eventoId, EventoDTO model)
        {
            try
            {
                var evento = await _eventosRepository.GetEventoByIdAsync(eventoId, false);
                if(evento == null) return null;

                model.EventoId = evento.EventoId;

                _mapper.Map(model, evento);

                _proEventosRepository.UpDate<Evento>(evento);

                if(await _proEventosRepository.SaveChangesAsync())
                {
                    var eventoRetorno = await _eventosRepository.GetEventoByIdAsync(eventoId, false);
                    
                    return _mapper.Map<EventoDTO>(eventoRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int eventoId)
        {
            try
            {
                var evento = await _eventosRepository.GetEventoByIdAsync(eventoId, false);

                _proEventosRepository.Delete<Evento>(evento);

                return await _proEventosRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDTO[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventosRepository.GetAllEventosAsync(includePalestrantes);
                if(eventos == null) throw new Exception ("Não há Eventos cadastrados.");

                var resultados = _mapper.Map<EventoDTO[]>(eventos);
                
                return resultados;
            }
            catch (Exception ex)
            {                
                throw new Exception (ex.Message);
            }
        }

        public async Task<List<EventoDTO>> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                 var evento = await _eventosRepository.GetAllEventosByTemaAsync(tema, includePalestrantes);

                 if(evento == null) throw new Exception("Tema não encontrado.");

                 var resultados = _mapper.Map<List<EventoDTO>>(evento);

                 return resultados;
            }
            catch (Exception ex)
            {                
                throw new Exception (ex.Message);
            }
        }

        public async Task<EventoDTO> GetEventoByIdAsync(int eventoid, bool includePalestrantes = false)
        {
            try
            {
                 var evento = await _eventosRepository.GetEventoByIdAsync(eventoid, includePalestrantes);

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