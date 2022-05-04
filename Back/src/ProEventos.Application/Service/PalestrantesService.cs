using System;
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
    public class PalestrantesService : IPalestrantesService
    {
        private readonly IPalestrantesRepository _palestrantesRepository;

        private readonly IMapper _mapper;
        public PalestrantesService(IPalestrantesRepository palestrantesRepository, 
                              IMapper mapper)
        {
            _palestrantesRepository = palestrantesRepository;
            _mapper = mapper;
        }

        public async Task<PalestranteDTO> AddPalestrantes(int userId, PalestranteAddDTO model)
        {
            try
            {
                var palestrante = _mapper.Map<Palestrante>(model);
                palestrante.UserId = userId;

                _palestrantesRepository.Add<Palestrante>(palestrante);
                
                if(await _palestrantesRepository.SaveChangesAsync())
                {
                    var palestranteRetorno = await _palestrantesRepository.GetPalestranteByUserIdAsync(userId, false);
                    return _mapper.Map<PalestranteDTO>(palestranteRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDTO> UpDatePalestrante(int userId, PalestranteUpdateDTO model)
        {
            try
            {
                var palestrante = await _palestrantesRepository.GetPalestranteByUserIdAsync(userId, false);
                if(palestrante == null) return null;

                model.Id = palestrante.Id;
                model.UserId = userId; 

                _mapper.Map(model, palestrante);

                _palestrantesRepository.UpDate<Palestrante>(palestrante);

                if(await _palestrantesRepository.SaveChangesAsync())
                {
                    var palestranteRetorno = await _palestrantesRepository.GetPalestranteByUserIdAsync(userId, false);
                    
                    return _mapper.Map<PalestranteDTO>(palestranteRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageList<PalestranteDTO>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false)
        {
            try
            {
                var palestrante = await _palestrantesRepository.GetAllPalestrantesAsync(pageParams, includeEventos);
                if(palestrante.Count == 0) throw new Exception ("Não há Palestrantes cadastrados.");

                var resultados = _mapper.Map<PageList<PalestranteDTO>>(palestrante);

                resultados.CurrentPage = palestrante.CurrentPage;
                resultados.TotalPages = palestrante.TotalPages;
                resultados.PageSize = palestrante.PageSize;
                resultados.TotalCount = palestrante.TotalCount;

                return resultados;
            }
            catch (Exception ex)
            {                
                throw new Exception (ex.Message);
            }
        }

        public async Task<PalestranteDTO> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false)
        {
            try
            {
                 var palestrante = await _palestrantesRepository.GetPalestranteByUserIdAsync(userId, includeEventos);

                 if(palestrante == null) return null;

                 var resultado = _mapper.Map<PalestranteDTO>(palestrante);
                 
                 return resultado;
            }
            catch (Exception ex)
            {                
                throw new Exception (ex.Message);
            }
        }
    }
}