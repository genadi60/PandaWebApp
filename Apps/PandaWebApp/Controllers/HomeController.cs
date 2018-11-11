using PandaWebApp.Services;

namespace PandaWebApp.Controllers
{
    using SIS.HTTP.Responses;

    public class HomeController : BaseController
    {
        private readonly IUsersService _usersService;

        public HomeController(IUsersService usersService)
        {
            _usersService = usersService;
        }
        public IHttpResponse Index()
        {
            if (!User.IsLoggedIn)
            {
                return View("/guest-home");
            }

            var model = _usersService.Profile(User.Username, Db);

            return this.View("/home/loggedInUser", model);
        }
    }
}
