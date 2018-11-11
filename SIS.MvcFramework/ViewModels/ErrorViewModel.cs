namespace SIS.MvcFramework.ViewModels
{
    public class ErrorViewModel
    {
        public ErrorViewModel(string message)
        {
            ErrorMessage = message;
            Error = message;
        }

        public string ErrorMessage { get; set; }
        
        public string Error { get; set; }
    }
}
