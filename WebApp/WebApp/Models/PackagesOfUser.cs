using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class PackagesOfUser
    {
        public int IdPackageUser { get; set; }
        public int PackageId { get; set; }
        public int UserId { get; set; }
        public string IdenticalNumber { get; set; } = null!;
    }
}
