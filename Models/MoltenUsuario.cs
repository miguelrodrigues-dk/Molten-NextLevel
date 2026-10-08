using System;
using System.ComponentModel.DataAnnotations;

namespace HelpDeskMvc.Models
{
    public class MoltenUsuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        public string Senha { get; set; } = string.Empty;

        public string TipoPlano { get; set; } = "Bronze"; 

        public string StatusAssinatura { get; set; } = "Ativo";

        [Required(ErrorMessage = "O perfil é obrigatório.")]
        public string Perfil { get; set; } = "Admin";


        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }
}
