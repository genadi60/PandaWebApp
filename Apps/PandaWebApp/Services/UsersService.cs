using System;
using System.Linq;
using PandaWebApp.Data;
using PandaWebApp.Models;
using PandaWebApp.ViewModels;
using SIS.MvcFramework;
using SIS.MvcFramework.Services;

namespace PandaWebApp.Services
{
    public class UsersService : IUsersService
    {
        private readonly IHashService _hashService;

        private readonly IPackagesService _packagesService;

        public UsersService(IHashService hashService, IPackagesService packagesService)
        {
            _hashService = hashService;
            _packagesService = packagesService;
        }

        public bool Find(string username, string password, PandaDbContext context)
        {
            using (var db = context)
            {
                return db
                    .Users
                    .Any(u => u.Username == username && u.Password == password);
            }
        }

        public UserViewModel Profile(string username, PandaDbContext context)
        {
            var model = new UserViewModel();

            using (var db = context)
            {
                var user = db.Users
                    .FirstOrDefault(u => u.Username.Equals(username));

                if (user != null)
                {
                    model.Id = user.Id;
                    model.Username = user.Username;
                    model.Role = user.Role.ToString();
                    model.Email = user.Email;
                    if (user.Role == Role.Admin)
                    {
                        model.Delivered = _packagesService.AdminDelivered(context);
                        model.Shipped = _packagesService.AdminShipped(context);
                        model.Pending = _packagesService.AdminPending(context);
                    }
                    else
                    {
                        model.Delivered = _packagesService.UserDelivered(username, context);
                        model.Shipped = _packagesService.UserShipped(username, context);
                        model.Pending = _packagesService.UserPending(username, context);
                    }
                }

                return model;
            }
        }
        
        public string GetUserId(string username, PandaDbContext context)
        {
            using (var db = context)
            {
                var id = db
                    .Users
                    .Where(u => u.Username == username)
                    .Select(u => u.Id)
                    .First();

                return id;
            }
        }

        public bool Create(RegisterViewModel model, PandaDbContext context)
        {
            using (var db = context)
            {
                if (db.Users.Any(u => u.Username == model.Username))
                {
                    return false;
                }

                var user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Username = model.Username,
                    Password = _hashService.Hash(model.Password),
                    Email = model.Email,
                };

                if (!db.Users.Any())
                {
                    user.Role = Role.Admin;
                }

                db.Add(user);
                db.SaveChanges();

                return true;
            }
        }

        public bool UserIsAuthenticated(LoginViewModel model, PandaDbContext context)
        {
            if (string.IsNullOrWhiteSpace(model.Username)
                || string.IsNullOrWhiteSpace(model.Password)
                || string.IsNullOrEmpty(model.Username) 
                || string.IsNullOrEmpty(model.Password))
            {
                return false;
            }

            var hashedPassword = _hashService.Hash(model.Password);

           
            
                if (!context.Users.Any(u => u.Password.Equals(hashedPassword) && u.Username.Equals(model.Username)))
                {
                    return false;
                }
           

            return true;
        }
    }
}
