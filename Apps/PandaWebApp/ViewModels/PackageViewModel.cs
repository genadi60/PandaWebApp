﻿namespace PandaWebApp.ViewModels
{
    public class PackageViewModel
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public double Weight { get; set; }

        public string ShippingAddress { get; set; }

        public string Status { get; set; }

        public string EstimatedDeliveryDate { get; set; }

        public string Recipient { get; set; }
    }
}
