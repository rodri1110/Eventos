using System.Collections.Generic;

namespace ProEventos.Application.DTOs
{
    public class PalestranteEventoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string MiniCurriculo { get; set; }
        public string ImagemURL { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public IEnumerable<RedeSocialDTO> RededesSociais { get; set; }
        public IEnumerable<PalestranteEventoDTO> PalestrantesEventos { get; set; }
    }
}