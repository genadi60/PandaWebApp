using System;
using System.Collections.Generic;
using System.Text;

namespace PandaWebApp.InputModels
{
    public class PackageInputModel
    {
        public ICollection<string> Recipients { get; set; } = new List<string>();
    }
}
