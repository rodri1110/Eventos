using System;
using System.Threading.Tasks;
using AutoMapper;
using System.Linq;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interface;
using ProEventos.Persistence.Repository.Interface;
using ProEventos.Domain;

namespace ProEventos.Application.Service
{
    public class RedesSociaisService : IRedesSociaisService
    {
        private readonly IRedeSocialRepository _context;
        private readonly IMapper _mapper;

        public RedesSociaisService(IRedeSocialRepository context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddRedeSocial(int id, RedeSocialDTO model, bool isEvento)
        {
            try
            {
                var redeSocial = _mapper.Map<RedeSocial>(model);

                if (isEvento)
                {
                    redeSocial.EventoId = id;
                    redeSocial.PalestranteId = null;
                }
                else
                {
                    redeSocial.PalestranteId = id;
                    redeSocial.EventoId = null;
                }

                _context.Add<RedeSocial>(redeSocial);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteByEvento(int eventoId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _context.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
                if (redeSocial == null) throw new Exception("Rede Social não encontrada.");

                _context.Delete<RedeSocial>(redeSocial);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteByPalestrante(int palestranteId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _context.GetRedeSocialPalestranteByIdsAsync(palestranteId, redeSocialId);
                if (redeSocial == null) throw new Exception("Rede Social não encontrada.");

                _context.Delete<RedeSocial>(redeSocial);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RedeSocialDTO[]> GetAllByEventoIdAsync(int eventoId)
        {
            try
            {
                var redesSociais = await _context.GetAllByEventoIdAsync(eventoId);
                if (redesSociais == null) return null;

                return _mapper.Map<RedeSocialDTO[]>(redesSociais);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RedeSocialDTO[]> GetAllByPalestranteIdAsync(int palestranteId)
        {
            try
            {
                var redesSociais = await _context.GetAllByPalestranteIdAsync(palestranteId);
                if (redesSociais == null) return null;

                return _mapper.Map<RedeSocialDTO[]>(redesSociais);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RedeSocialDTO> GetRedeSocialEventoByIdsAsync(int eventoId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _context.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
                if (redeSocial == null) return null;

                return _mapper.Map<RedeSocialDTO>(redeSocial);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RedeSocialDTO> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _context.GetRedeSocialPalestranteByIdsAsync(palestranteId, redeSocialId);
                if(redeSocial == null) return null;

                return _mapper.Map<RedeSocialDTO>(redeSocial);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RedeSocialDTO[]> SaveByEvento(int eventoId, RedeSocialDTO[] models)
        {
            try
            {
                var redesSociais = await _context.GetAllByEventoIdAsync(eventoId);
                if (redesSociais == null) return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddRedeSocial(eventoId, model, true);
                    }
                    else
                    {
                        var redeSocial = redesSociais.FirstOrDefault(rs => rs.Id == model.Id);
                        model.EventoId = eventoId;

                        var modelMapper = _mapper.Map<RedeSocial>(model);

                        _context.UpDate<RedeSocial>(modelMapper);
                        await _context.SaveChangesAsync();
                    }
                }

                var redeSocialRetorno = await _context.GetAllByEventoIdAsync(eventoId);

                return _mapper.Map<RedeSocialDTO[]>(redeSocialRetorno);

            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<RedeSocialDTO[]> SaveByPalestrante(int palestranteid, RedeSocialDTO[] models)
        {
            try
            {
                var redesSociais = await _context.GetAllByPalestranteIdAsync(palestranteid);
                if (redesSociais == null) return null;

                foreach (var model in models)
                {

                    if (model.Id == 0)
                    {
                        await AddRedeSocial(palestranteid, model, false);
                    }
                    else
                    {
                        var redeSocial = redesSociais.FirstOrDefault(rs => rs.Id == model.Id);
                        model.PalestranteId = palestranteid;

                        _mapper.Map(model, redeSocial);

                        _context.UpDate<RedeSocial>(redeSocial);
                        await _context.SaveChangesAsync();
                    }
                }

                var redeSocialRetorno = await _context.GetAllByPalestranteIdAsync(palestranteid);
                return _mapper.Map<RedeSocialDTO[]>(redeSocialRetorno);                
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}