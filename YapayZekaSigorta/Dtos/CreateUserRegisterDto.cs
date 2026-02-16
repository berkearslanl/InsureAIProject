using System.ComponentModel.DataAnnotations;

namespace YapayZekaSigorta.Dtos
{
    public class CreateUserRegisterDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [Required(ErrorMessage = "Kullanım şartlarını kabul etmeniz gerekmektedir.")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Kullanım şartlarını kabul etmeniz gerekiyor.")]
        public bool TermsAccepted { get; set; }
    }
}
