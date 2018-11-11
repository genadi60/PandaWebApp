using System.Collections.Generic;
using System.Linq;
using PandaWebApp.Models;
using PandaWebApp.Services;
using PandaWebApp.ViewModels;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using SIS.MvcFramework.ViewModels;

namespace PandaWebApp.Controllers
{
    public class ReceiptsController : BaseController
    {
        private readonly IReceiptsService _receiptsService;

        private readonly IPackagesService _packagesService;

        private readonly IUsersService _usersService;

        public ReceiptsController(IReceiptsService receiptsService, IPackagesService packagesService, IUsersService usersService)
        {
            _receiptsService = receiptsService;
            _packagesService = packagesService;
            _usersService = usersService;
        }

        public IHttpResponse Index()
        {
            if (!User.IsLoggedIn)
            {
                return View("/users/login");
            }

            var userViewModel = new UserViewModel
            {
                Receipts = User.Role.Equals(Role.Admin.ToString())
                ? _receiptsService.FindAll(Db)
                : _receiptsService.FindByRecipient(User.Username, Db)
            };

            return View("/receipts", userViewModel);
        }

        [HttpPost]
        public IHttpResponse Index(int id)
        {
            if (!User.IsLoggedIn)
            {
                return View("/users/login");
            }

            var packageViewModel = _packagesService.FindById(id, Db);

            if (User.Role.Equals(Role.User.ToString()) && !packageViewModel.Recipient.Equals(User.Username))
            {
                return View("/");
            }

            if (!_receiptsService.Acquire(id, Db))
            {
                var errorMessage = "Package not found.";
                return View("/error", new ErrorViewModel(errorMessage));
            }

            var userViewModel = _usersService.Profile(User.Username, Db);

            return View("/home/loggedInUser", userViewModel);
        }

        public IHttpResponse Details(int id)
        {
            if (!User.IsLoggedIn)
            {
                return View("/users/login");
            }

            var receiptViewModel = _receiptsService.FindById(id, Db);

            return View("/details-receipt", receiptViewModel);
        }
    }
}
