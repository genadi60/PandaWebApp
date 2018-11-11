using System;
using System.Collections.Generic;
using System.Text;
using PandaWebApp.Data;
using PandaWebApp.ViewModels;
using SIS.MvcFramework;

namespace PandaWebApp.Services
{
    public interface IUsersService
    {
        bool Create(RegisterViewModel model, PandaDbContext context);

        bool UserIsAuthenticated(LoginViewModel model,  PandaDbContext context);

        bool Find(string username, string password,  PandaDbContext context);

        UserViewModel Profile(string username,  PandaDbContext context);

        int GetUserId(string username,  PandaDbContext context);
    }
}
