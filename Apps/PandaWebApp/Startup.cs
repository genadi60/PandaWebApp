﻿using PandaWebApp.Services;

namespace PandaWebApp
{
    using SIS.MvcFramework;
    using SIS.MvcFramework.Logger;
    using SIS.MvcFramework.Services;

    public class Startup : IMvcApplication
    {
        public MvcFrameworkSettings Configure()
        {
            return new MvcFrameworkSettings { PortNumber = 80 };
        }

        public void ConfigureServices(IServiceCollection collection)
        {
            collection.AddService<ILogger, ConsoleLogger>();
            collection.AddService<IUsersService, UsersService>();
            collection.AddService<IPackagesService, PackagesService>();
            collection.AddService<IReceiptsService, ReceiptsService>();
        }
    }
}
