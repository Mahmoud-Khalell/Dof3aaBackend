
using PresentationLayer.DTO.Material;

namespace PresentationLayer.DTO.Topic
{
    public class TopicInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int CourceId { get; set; }
        public DateTime? LastUpdate { get; set; }

        public List<Material_Info> Materials { get; set; }


    }
}
