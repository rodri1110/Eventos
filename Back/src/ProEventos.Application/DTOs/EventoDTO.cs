using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace ProEventos.Application.DTOs
{
    public class EventoDTO
    {
        public int EventoId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Local { get; set; }
        
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public DateTime DataEvento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório."),
        MinLength(3, ErrorMessage = "O campo deve ter no mínimo 4 caracteres."),
        MaxLength(50, ErrorMessage = "O campo deve ter no máximo 50 caracteres.")]
        //[StringLength(50, MinimumLength = 3, ErrorMessage ="O valor deve estar entre 3 e 50 caracteres.")]
        public string Tema { get; set; }

        [Display(Name = "Qtd Pessoas")]
        [Range(5, 500, ErrorMessage = "O campo {0} deve conter entre {1} e {2} pessoas.")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int QtdPessoas { get; set; }

        [RegularExpression(@".*\.(gif|jpe?g|bmp|png)$", ErrorMessage = 
        "Não é uma imagem válida. (gif, jpg, jpeg, bmp ou png)")]
        public string ImagemURL { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Phone]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "e-mail")]
        [EmailAddress(ErrorMessage = "O campo {0} precisa ser válido.")]
        public string Email { get; set; }

        public IEnumerable<LoteDTO> Lotes { get; set; }
        public IEnumerable<RedeSocialDTO> RedesSociais { get; set; }
        public IEnumerable<PalestranteEventoDTO> PalestrantesEventos { get; set; }
    }
}