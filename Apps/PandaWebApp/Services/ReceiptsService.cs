using System;
using System.Collections.Generic;
using System.Linq;
using PandaWebApp.Data;
using PandaWebApp.Models;
using PandaWebApp.ViewModels;

namespace PandaWebApp.Services
{
    public class ReceiptsService : IReceiptsService
    {
        public bool Acquire(string id, PandaDbContext context)
        {
            var package = context.Packages.FirstOrDefault(p => p.Id == id);

            if (package == null)
            {
                return false;
            }

            package.Status = Status.Acquired;

            context.Packages.Update(package);
            context.SaveChanges();


            var receipt = new Receipt();
            receipt.Id = Guid.NewGuid().ToString();
            receipt.Fee = (decimal)(package.Weight * 2.67);
            receipt.Recipient = package.Recipient;
            receipt.Package = package;
            

            context.Receipts.Add(receipt);
            context.SaveChanges();

            return true;
        }

        public ReceiptViewModel FindById(string id, PandaDbContext context)
        {
            var receiptViewModel = context.Receipts
                .Select(r => new ReceiptViewModel
                {
                    Id = r.Id,
                    Fee = r.Fee,
                    IssuedOn = r.IssuedOn.ToString("dd/MM/yyyy"),
                    Package = r.Package,
                    Recipient = r.Recipient.Username
                })
                .FirstOrDefault(p => p.Id == id);

            return receiptViewModel;
        }

        public IList<ReceiptViewModel> FindAll(PandaDbContext context)
        {
            var receipts = context.Receipts
                .Select(r => new ReceiptViewModel
                {
                    Id = r.Id,
                    Fee = r.Fee,
                    IssuedOn = r.IssuedOn.ToString("dd/MM/yyyy"),
                    Package = r.Package,
                    Recipient = r.Recipient.Username
                })
                .ToList();

            return receipts;
        }

        public IList<ReceiptViewModel> FindByRecipient(string username, PandaDbContext context)
        {
            var receiptViewModel = context.Receipts
                .Where(r => r.Recipient.Username.Equals(username))
                .Select(r => new ReceiptViewModel
                {
                    Id = r.Id,
                    Fee = r.Fee,
                    IssuedOn = r.IssuedOn.ToString("dd/MM/yyyy"),
                    Package = r.Package,
                    Recipient = r.Recipient.Username
                })
                .ToList();

            return receiptViewModel;
        }
    }
}
