using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.entities
{
    public class AppUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }

        public string? University {  get; set; }
        public string? Department { get; set; }
        public string? faculty {  get; set; }
        public DateTime DateOfCreation { get; set; }
        public string ImageUrl { get; set; }
        public virtual ICollection<UserGroup> UserGroups { get; set; }  
        public virtual ICollection<UserNotification> UserNotifications { get; set; }
        public AppUser()
        {
            DateOfCreation = DateTime.Now;
            
        }
    }
}
