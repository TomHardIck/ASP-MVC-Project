using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class UserRole
    {
        public UserRole()
        {
            Users = new HashSet<User>();
        }

        public int IdRole { get; set; }
        public string RoleName { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
