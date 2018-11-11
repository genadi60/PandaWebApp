﻿using System.Collections.Generic;
using PandaWebApp.Data;
using PandaWebApp.ViewModels;
using SIS.MvcFramework;

namespace PandaWebApp.Services
{
    public interface IPackagesService
    {
        bool CreatePackage(PackageViewModel model, PandaDbContext context);

        PackageViewModel FindById(int id, PandaDbContext context);

        ICollection<string> GetRecipients(PandaDbContext context);

        IList<PackageViewModel> AdminPending(PandaDbContext context);

        IList<PackageViewModel> UserPending(string username, PandaDbContext context);

        IList<PackageViewModel> AdminShipped(PandaDbContext context);

        IList<PackageViewModel> UserShipped(string username, PandaDbContext context);

        IList<PackageViewModel> AdminDelivered(PandaDbContext context);

        IList<PackageViewModel> UserDelivered(string username, PandaDbContext context);

        bool Ship(int id, PandaDbContext context);

        bool Deliver(int id, PandaDbContext context);
    }
}
