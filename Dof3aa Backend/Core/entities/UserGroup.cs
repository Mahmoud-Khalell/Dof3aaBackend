using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.entities
{
    public  class UserGroup:BaseEntity
    {
        
        [ForeignKey("User")]
        public string Username { get; set; }
        
        [ForeignKey("Cource")]
        public int CourceId { get; set; }
        public int rule { get; set; }
        public virtual AppUser User { get; set; }
        public virtual Cource Cource { get; set; }




    }
}
