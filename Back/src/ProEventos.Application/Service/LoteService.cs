using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interface;
using ProEventos.Domain;
using ProEventos.Persistence.Interface;
using ProEventos.Persistence.Repository.Interface;

namespace ProEventos.Application.Service
{
    public class LoteService : ILoteService
    {
        private readonly ILoteRepository _loteRepository;
        private readonly IProEventosRepository _proEventosRepository;

        private readonly IMapper _mapper;
        public LoteService(IProEventosRepository proEventosRepository,
                            ILoteRepository loteRepository, 
                            IMapper mapper)
        {
            _proEventosRepository = proEventosRepository;
            _loteRepository = loteRepository;
            _mapper = mapper;
        }

        public async Task AddLote(int eventoId, LoteDTO model)
        {
            try
            {
                var lote = _mapper.Map<Lote>(model);
                lote.EventoId = eventoId;

                _proEventosRepository.Add<Lote>(lote);
                
                await _proEventosRepository.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public async Task<LoteDTO[]> SaveLotes(int eventoId, LoteDTO[] models)
        {
            try
            {
                var lotes = await _loteRepository.GetLotesByEventoIdAsync(eventoId);
                if(lotes == null) throw new Exception ("Não foi possível salvar lote");

                foreach (var model in models)
                {
                    if(model.Id == 0){
                        await AddLote(eventoId, model);
                    } else {
                        
                        var lote = lotes.FirstOrDefault(lote => lote.Id == model.Id);

                        model.EventoId = eventoId;

                        _mapper.Map(model, lote);

                        _proEventosRepository.UpDate<Lote>(lote);

                        await _proEventosRepository.SaveChangesAsync();
                    }                    
                        
                }
                
                var loteRetorno = await _loteRepository.GetLotesByEventoIdAsync(eventoId);
                    
                return _mapper.Map<LoteDTO[]>(loteRetorno);

            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteLote(int eventoId, int loteId)
        {
            try
            {
                var lote = await _loteRepository.GetLoteByIdsAsync(eventoId, loteId);
                if(lote == null) throw new Exception ("Lote não encontrado");

                _proEventosRepository.Delete<Lote>(lote);

                return await _proEventosRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDTO[]> GetLotesByEventoIdAsync(int eventoId)
        {
            try
            {
                 var lotes = await _loteRepository.GetLotesByEventoIdAsync(eventoId);

                 if(lotes == null) throw new Exception("Tema não encontrado.");

                 var resultados = _mapper.Map<LoteDTO[]>(lotes);

                 return resultados;
            }
            catch (Exception ex)
            {                
                throw new Exception (ex.Message);
            }
        }

        public async Task<LoteDTO> GetLoteByIdsAsync(int eventoId, int loteId)
        {
            try
            {
                 var lote = await _loteRepository.GetLoteByIdsAsync(eventoId, loteId);

                 if(lote == null) throw new Exception("Evento não encontrado.");

                 var resultado = _mapper.Map<LoteDTO>(lote);
                 
                 return resultado;
            }
            catch (Exception ex)
            {                
                throw new Exception (ex.Message);
            }
        }
    }
}