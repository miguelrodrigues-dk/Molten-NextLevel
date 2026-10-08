using System;
using System.ComponentModel.DataAnnotations;

namespace HelpDeskMvc.Models
{
    public class Filme
    {
       
        public int Id { get; set; }
      
        [Required(ErrorMessage = "O título é obrigatório.")]
        [Display(Name = "Título do Filme")]
        public string? Titulo { get; set; }

   
        [Required(ErrorMessage = "A Sinopse é obrigatório.")]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        public string? Status { get; set; } = "Nos Cinemas";

        [Display(Name = "Data de Cadastro")]
        public DateTime DataAbertura { get; set; } = DateTime.Now;

        [Display(Name = "Data de Encerramento")]
        public DateTime? DataFechamento { get; set; }

    }
    
    }
