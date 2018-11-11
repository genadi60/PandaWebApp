using PandaWebApp.ViewModels;
using SIS.HTTP.Responses;
using PandaWebApp.Services;
using SIS.MvcFramework;
using System.Linq;
using SIS.MvcFramework.ViewModels;
using SIS.HTTP.Cookies;
using SIS.HTTP.Common;

namespace PandaWebApp.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUsersService _usersService;
        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public IHttpResponse Login()
        {
            if (!Db.Users.Any())
            {
                var errorMessage = "First must to register.";
                return View("/error", new ErrorViewModel(errorMessage));
            }
            return View("/login");
        }

        [HttpPost]
        public IHttpResponse Login(LoginViewModel model)
        {
            if (!_usersService.UserIsAuthenticated(model, Db))
            {
                var errorMessage = "Invalid username or password.";
                return View("error", new ErrorViewModel(errorMessage));
            }

            var userViewModel = _usersService.Profile(model.Username, Db);

            var mvcUser = new MvcUserInfo { Username = userViewModel.Username, Role = userViewModel.Role, Info = userViewModel.Email };

            var cookieContent = UserCookieService.GetUserCookie(mvcUser);

            Request.Session.AddParameter(".auth_cake", cookieContent);

            Response.Cookies.Add(new HttpCookie(".auth_cake", $"{cookieContent}; {GlobalConstants.HttpOnly}", 7));

            
            return View("/home/loggedInUser", userViewModel);
        }

       public IHttpResponse Register()
        {
            return View("/register");
        }

        [HttpPost]
        public IHttpResponse Register(RegisterViewModel model)
        {
            string errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(model.Username) || model.Username.Length < 4)
            {
                errorMessage = "Please, provide valid username with length 4 or more symbols";
            }

            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length < 6)
            {
                errorMessage = "Please, provide valid password with length 6 or more symbols";
            }

            if (!model.Password.Equals(model.ConfirmPassword))
            {
                errorMessage = "Passwords do not match.";
            }

            if (!errorMessage.Equals(string.Empty))
            {
                return View("/error", new ErrorViewModel(errorMessage));
            }

            bool isRegistered = _usersService.Create(model, Db);

            if (!isRegistered)
            {
                errorMessage = $"User with username: {model.Username} already exists.";
            }

            if (!errorMessage.Equals(string.Empty))
            {
                return View("/error", new ErrorViewModel(errorMessage));
            }

            return View("/");
        }


        public IHttpResponse Logout()
        {
            Request.Session.ClearParameters();

            if (User != null)
            {
                var cookie = Request.Cookies.GetCookie(".auth_cake");

                cookie.Delete();

                Response.Cookies.Add(cookie);
            }

            return Redirect("/");
        }
    }
}
