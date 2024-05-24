using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.DTO
{
    public class RegisterationDTO
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? University { get; set; }
        public string? Department { get; set; }
        public string? Faculty { get; set; }
        
        public IFormFile?  Image { get; set; }

        [EmailAddress]
        public string Email {  get; set; }
        [Required]
        public string Username {  get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string PasswordConfirmation { get; set; }

        

    }
}
