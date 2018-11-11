using System.Collections.Generic;


namespace PandaWebApp.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public virtual IList<ReceiptViewModel> Receipts { get; set; } = new List<ReceiptViewModel>();

        public virtual IList<PackageViewModel> Pending { get; set; } = new List<PackageViewModel>();

        public virtual IList<PackageViewModel> Shipped { get; set; } = new List<PackageViewModel>();

        public virtual IList<PackageViewModel> Delivered { get; set; } = new List<PackageViewModel>();
    }
}
