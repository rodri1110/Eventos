using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public EventosService(IEventosRepository eventosRepository, IProEventosRepository proEventosRepository)
        {
            _proEventosRepository = proEventosRepository;
            _eventosRepository = eventosRepository;
        }
        public async Task<Evento> AddEvento(Evento model)
        {
            try
            {
                _proEventosRepository.Add(model);
                
                if(await _proEventosRepository.SaveChangesAsync())
                {
                    return await _eventosRepository.GetEventoByIdAsync(model.EventoId, false);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Evento> UpDateEvento(int eventoId, Evento model)
        {
            try
            {
                var evento = await _eventosRepository.GetEventoByIdAsync(eventoId, false);
                if(evento == null) return null;

                _proEventosRepository.UpDate(model);

                if(await _proEventosRepository.SaveChangesAsync())
                {
                    return await _eventosRepository.GetEventoByIdAsync(model.EventoId, false);
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
                if(evento == null) throw new Exception("Evento não encontrado.");

                _proEventosRepository.Delete<Evento>(evento);

                return await _proEventosRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventosRepository.GetAllEventosAsync(includePalestrantes);
                if(eventos == null) throw new Exception ("Não há Eventos cadastrados.");

                return eventos;
            }
            catch (Exception ex)
            {                
                throw new Exception (ex.Message);
            }
        }

        public async Task<List<Evento>> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                 var evento = await _eventosRepository.GetAllEventosByTemaAsync(tema, includePalestrantes);

                 if(evento == null) throw new Exception("Tema não encontrado.");

                 return evento;
            }
            catch (Exception ex)
            {                
                throw new Exception (ex.Message);
            }
        }

        public async Task<Evento> GetEventoByIdAsync(int eventoid, bool includePalestrantes = false)
        {
            try
            {
                 var evento = await _eventosRepository.GetEventoByIdAsync(eventoid, includePalestrantes);

                 if(evento == null) throw new Exception("Evento não encontrado.");

                 return evento;
            }
            catch (Exception ex)
            {                
                throw new Exception (ex.Message);
            }
        }
    }
}