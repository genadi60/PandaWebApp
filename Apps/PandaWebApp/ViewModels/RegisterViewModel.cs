using PandaWebApp.Models;

namespace PandaWebApp.ViewModels
{
    public class RegisterViewModel
    {
        public RegisterViewModel()
        {
            
        }
        public string Name { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Email { get; set; }
    }
}
