using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class User
    {
        public int IdUser { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string TgLink { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }

        public virtual UserRole Role { get; set; } = null!;

        public string Identical_Number { get; set; } = null!;
    }
}
