using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Package
    {
        public int IdPackage { get; set; }
        public string PackageName { get; set; } = null!;
        public string ProductArt { get; set; } = null!;
        public string TypeDelivery { get; set; } = null!;
        public string ProductLink { get; set; } = null!;
        public int ProductCount { get; set; }
        public double ProductPrice { get; set; }
        public bool IsFinished { get; set; }
        public string TrackNumber { get; set; } = null!;
    }
}
