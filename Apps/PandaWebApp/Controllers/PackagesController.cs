using System.Linq;
using PandaWebApp.InputModels;
using PandaWebApp.Models;
using PandaWebApp.Services;
using PandaWebApp.ViewModels;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using SIS.MvcFramework.ViewModels;

namespace PandaWebApp.Controllers
{
    public class PackagesController : BaseController
    {
        private readonly IPackagesService _packagesService;

        private readonly IUsersService _usersService;

        public PackagesController(IPackagesService packagesService, IUsersService usersService)
        {
            _packagesService = packagesService;
            _usersService = usersService;
        }

        public IHttpResponse Create(PackageInputModel model)
        {
            if (User.Role != Role.Admin.ToString())
            {
                return Redirect("/");
            }

            model.Recipients = _packagesService.GetRecipients(Db);

            return View("/create", model);
        }

        [HttpPost]
        public IHttpResponse Create(PackageViewModel model)
        {
            
            if (User.Role != Role.Admin.ToString())
            {
                return View("/");
            }

            if (!_packagesService.CreatePackage(model, Db))
            {
                var errorMessage = $"Package : {model.Description} not created";
                return View("/error", new ErrorViewModel(errorMessage));
            }

            var userViewModel = _usersService.Profile(User.Username, Db);

            return View("/home/loggedInUser", userViewModel);
        }

        public IHttpResponse Pending()
        {
            if (User.Role != Role.Admin.ToString())
            {
                return View("/");
            }

            var userViewModel = new UserViewModel
            {
                Pending = _packagesService.AdminPending(Db)
            };

            return View("/pending-packages", userViewModel);
        }

        public IHttpResponse Shipped()
        {
            if (User.Role != Role.Admin.ToString())
            {
                return View("/");
            }

            var userViewModel = new UserViewModel();

            userViewModel.Shipped = _packagesService.AdminShipped(Db);
            
            return View("/shipped-packages", userViewModel);
        }

        public IHttpResponse Delivered()
        {
            if (User.Role != Role.Admin.ToString())
            {
                return View("/");
            }

            var userViewModel = new UserViewModel
            {
                Delivered = _packagesService.AdminDelivered(Db)
            };

            return View("/delivered-packages", userViewModel);
        }

        public IHttpResponse Details(int id)
        {
            if (!User.IsLoggedIn)
            {
                return View("/users/login");
            }

            var package = Db.Packages.FirstOrDefault(p => p.Id == id);

            if (package == null)
            {
                var errorMessage = "Package not found.";
                return View("/", new ErrorViewModel(errorMessage));
            }

            var model = new PackageViewModel
            {
                Id = package.Id,
                Description = package.Description,
                Weight = package.Weight,
                ShippingAddress = package.ShippingAddress,
                Status = package.Status.ToString(),
                EstimatedDeliveryDate = package.EstimatedDeliveryDate != null ? package.EstimatedDeliveryDate.Value.ToString("dd/MM/yyyy") : "N/A",
                Recipient = package.Recipient.Username
            };

            return View("/details-package", model);
        }

        public IHttpResponse Ship(int id)
        {
            if (User.Role != Role.Admin.ToString())
            {
                return Redirect("/");
            }
            if (!_packagesService.Ship(id, Db))
            {
                var errorMessage = "Package not found.";
                return View("/error", new ErrorViewModel(errorMessage));
            }

            var model = _usersService.Profile(User.Username, Db);

            return View("/home/loggedInUser", model);
        }

        public IHttpResponse Deliver(int id)
        {
            if (User.Role != Role.Admin.ToString())
            {
                return Redirect("/");
            }
            if (!_packagesService.Deliver(id, Db))
            {
                var errorMessage = "Package not found.";
                return View("/error", new ErrorViewModel(errorMessage));
            }

            var model = _usersService.Profile(User.Username, Db);

            return View("/home/loggedInUser", model);
        }
    }
}
