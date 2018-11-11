using System;
using System.Collections.Generic;
using System.Text;

namespace PandaWebApp.ViewModels
{
    public class AllReceiptsViewModel
    {
        public virtual IList<ReceiptViewModel> ReceiptViewModels { get; set; }
    }
}
