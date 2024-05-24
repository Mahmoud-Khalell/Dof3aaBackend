using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.entities
{
    public class Notification:BaseEntity
    {
        public string description { get; set; }
        public DateTime CreationDate { get; set; }
        [ForeignKey("User")]
        public string publiserUsername { get; set; }
        public virtual AppUser User { get; set; }


    }
}
