using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.entities
{
    public class Material:BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string FileUrl { get; set; }
        [ForeignKey("Topic")]
        public int TopicId { get; set; }
        public DateTime? PublishDate { get; set; }
        public int? Type { get; set; }
        public virtual Topic Topic { get; set; }
        public Material()
        {
            PublishDate = DateTime.Now;
        }
    }
}
