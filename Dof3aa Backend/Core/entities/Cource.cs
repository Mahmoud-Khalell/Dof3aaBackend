using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.entities
{
    public class Cource:BaseEntity
    {

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string ?SubTitle { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; } 
        public int type { get; set; }
        public virtual ICollection<UserGroup> UserGroups { get; set; }  
        public virtual ICollection<Topic> Topics { get; set; }
        public virtual ICollection<Announcement> Announcements { get; set; }
        public virtual ICollection<task> Tasks { get; set; }
    }
}
