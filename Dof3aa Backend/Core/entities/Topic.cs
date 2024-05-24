using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.entities
{
    public class Topic: BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        [ForeignKey("Cource")]
        public int CourseId { get; set; }
        public virtual Cource Cource { get; set; }
        public DateTime? CretaedAt { get; set; }
        public virtual ICollection<Material> Materials { get; set; }
        public Topic()
        {
            CretaedAt = DateTime.Now;
        }




    }
}
