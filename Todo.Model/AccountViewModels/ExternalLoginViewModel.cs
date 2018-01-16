using System.ComponentModel.DataAnnotations;

namespace Todo.Model.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
