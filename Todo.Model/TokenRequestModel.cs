using System.ComponentModel.DataAnnotations;

namespace Todo.Model
{
    public class TokenRequestModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Issuer { get; set; }

        [Required]
        public string Audience { get; set; }
    }
}
