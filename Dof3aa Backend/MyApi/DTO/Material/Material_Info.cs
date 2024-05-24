namespace PresentationLayer.DTO.Material
{
    public class Material_Info
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SaurceUrl { get; set; }
        public int? Type { get; set; }
        public int TopicId { get; set; }
        public DateTime? CreatedAt { get; set; }


    }
}
