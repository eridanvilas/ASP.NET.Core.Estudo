using System.ComponentModel.DataAnnotations;

namespace WebApp.Exercicio.Models
{
    public class TwoFactorModel
    {
        [Required]
        public string Token { get; set; }
    }
}
