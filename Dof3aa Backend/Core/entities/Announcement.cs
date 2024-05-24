using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.entities
{
    public class Announcement:BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        [ForeignKey("Cource")]
        public int CourceId { get; set; }
        public DateTime CreateDate { get; set; }
        [ForeignKey("PublisherUser")]
        public string PublisherUserName { get; set; }
        public virtual Cource Cource { get; set; }
        public virtual AppUser PublisherUser { get; set; }
        public Announcement()
        {
            CreateDate = DateTime.Now;
        }
    }
}
