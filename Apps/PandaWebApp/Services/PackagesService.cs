using System;
using System.Collections.Generic;
using System.Linq;
using PandaWebApp.Data;
using PandaWebApp.Models;
using PandaWebApp.ViewModels;
using SIS.MvcFramework;

namespace PandaWebApp.Services
{
    public class PackagesService : IPackagesService
    {
        public bool CreatePackage(PackageViewModel model, PandaDbContext context)
        {
            if (model == null)
            {
                return false;
            }
            var package = new Package
            {
                Id = Guid.NewGuid().ToString(),
                Description = model.Description,
                Recipient = context.Users.FirstOrDefault(u => u.Username.Equals(model.Recipient)),
                Weight = model.Weight,
                ShippingAddress = model.ShippingAddress,
                Status = Status.Pending,
                EstimatedDeliveryDate = null
            };
            
            context.Packages.Add(package);
            context.SaveChanges();

            return true;
        }

        public PackageViewModel FindById(string id, PandaDbContext context)
        {
            var packageViewModel = context.Packages
                .Select(p => new PackageViewModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    EstimatedDeliveryDate = p.EstimatedDeliveryDate != null
                        ? p.EstimatedDeliveryDate.Value.ToString("dd/MM/yyyy")
                        : "N/A",
                    Status = p.Status.ToString(),
                    ShippingAddress = p.ShippingAddress,
                    Weight = p.Weight,
                    Recipient = p.Recipient.Username
                })
                .FirstOrDefault(p => p.Id == id);

            return packageViewModel;
        }

        public ICollection<string> GetRecipients(PandaDbContext context)
        {
            var recients = context.Users
                .Select(u => u.Username)
                .ToList();

            return recients;
        }

        public IList<PackageViewModel> AdminPending(PandaDbContext context)
        {
            var pendingPackages = context.Packages
                .Where(p => p.Status == Status.Pending)
                .Select(p => new PackageViewModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    Weight  = p.Weight,
                    ShippingAddress = p.ShippingAddress,
                    Status = p.Status.ToString(),
                    EstimatedDeliveryDate = "N/A",
                    Recipient = p.Recipient.Username
                })
                .ToList();

            return pendingPackages;
        }

        public IList<PackageViewModel> UserPending(string username, PandaDbContext context)
        {
            var pendingPackages = context.Packages
                .Where(p => p.Recipient.Username.Equals(username) && p.Status == Status.Pending)
                .Select(p => new PackageViewModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    Weight  = p.Weight,
                    ShippingAddress = p.ShippingAddress,
                    Status = p.Status.ToString(),
                    EstimatedDeliveryDate = "N/A",
                    Recipient = p.Recipient.Username
                })
                .ToList();

            return pendingPackages;
        }

        public IList<PackageViewModel> AdminShipped(PandaDbContext context)
        {
            var shippedPackages = context.Packages
                .Where(p => p.Status == Status.Shipped)
                .ToArray();

            bool isDelivered = false;

            foreach (var package in shippedPackages)
            {
                if (package.EstimatedDeliveryDate > DateTime.UtcNow)
                {
                    continue;
                }

                package.Status = Status.Delivered;
                isDelivered = true;
            }

            if (isDelivered)
            {
                context.Packages.UpdateRange(shippedPackages);
                context.SaveChanges();
            }
            
            var packages = context.Packages
                .Where(p => p.Status == Status.Shipped)
                .Select(p => new PackageViewModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    Weight  = p.Weight,
                    ShippingAddress = p.ShippingAddress,
                    Status = p.Status.ToString(),
                    EstimatedDeliveryDate = p.EstimatedDeliveryDate.Value.ToString("dd/MM/yyyy"),
                    Recipient = p.Recipient.Username
                })
                .ToList();

            return packages;
        }

        public IList<PackageViewModel> UserShipped(string username, PandaDbContext context)
        {
            var shippedPackages = context.Packages
                .Where(p => p.Recipient.Username.Equals(username) && p.Status == Status.Shipped)
                .ToArray();

            bool isDelivered = false;

            foreach (var package in shippedPackages)
            {
                if (package.EstimatedDeliveryDate > DateTime.UtcNow)
                {
                    continue;
                }

                package.Status = Status.Delivered;
                isDelivered = true;
            }

            if (isDelivered)
            {
                context.Packages.UpdateRange(shippedPackages);
                context.SaveChanges();
            }
            
            var packages = context.Packages
                .Where(p => p.Recipient.Username.Equals(username) && p.Status == Status.Shipped)
                .Select(p => new PackageViewModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    Weight  = p.Weight,
                    ShippingAddress = p.ShippingAddress,
                    Status = p.Status.ToString(),
                    EstimatedDeliveryDate = p.EstimatedDeliveryDate.Value.ToString("dd/MM/yyyy"),
                    Recipient = p.Recipient.Username
                })
                .ToList();

            return packages;
        }

        public IList<PackageViewModel> AdminDelivered(PandaDbContext context)
        {
            var deliveredPackages = context.Packages
                .Where(p => p.Status == Status.Delivered || p.Status == Status.Acquired)
                .Select(p => new PackageViewModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    Weight  = p.Weight,
                    ShippingAddress = p.ShippingAddress,
                    Status = p.Status.ToString(),
                    EstimatedDeliveryDate = "Delivered",
                    Recipient = p.Recipient.Username
                })
                .ToList();

            return deliveredPackages;
        }

        public IList<PackageViewModel> UserDelivered(string username, PandaDbContext context)
        {
            var deliveredPackages = context.Packages
                .Where(p => p.Recipient.Username.Equals(username) && p.Status == Status.Delivered)
                .Select(p => new PackageViewModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    Weight  = p.Weight,
                    ShippingAddress = p.ShippingAddress,
                    Status = p.Status.ToString(),
                    EstimatedDeliveryDate = "Delivered",
                    Recipient = p.Recipient.Username
                })
                .ToList();

            return deliveredPackages;
        }

        public bool Ship(string id, PandaDbContext context)
        {
            var package = context.Packages.FirstOrDefault(p => p.Id == id);

            if (package == null)
            {
                return false;
            }

            package.Status = Status.Shipped;
            var random = new Random();
            var addDays = random.Next(20, 41);
            package.EstimatedDeliveryDate = DateTime.UtcNow.AddDays(addDays);

            context.Packages.Update(package);
            context.SaveChanges();

            return true;
        }

        public bool Deliver(string id, PandaDbContext context)
        {
            var package = context.Packages.FirstOrDefault(p => p.Id == id);

            if (package == null)
            {
                return false;
            }

            package.Status = Status.Delivered;

            context.Packages.Update(package);
            context.SaveChanges();

            return true;
        }
    }
}
