using System.Collections.Generic;
using PandaWebApp.Data;
using PandaWebApp.Models;
using PandaWebApp.ViewModels;

namespace PandaWebApp.Services
{
    public interface IReceiptsService
    {
        bool Acquire(string id, PandaDbContext context);

        ReceiptViewModel FindById(string id, PandaDbContext context);

        IList<ReceiptViewModel> FindAll(PandaDbContext context);

        IList<ReceiptViewModel> FindByRecipient(string username, PandaDbContext context);
    }
}
