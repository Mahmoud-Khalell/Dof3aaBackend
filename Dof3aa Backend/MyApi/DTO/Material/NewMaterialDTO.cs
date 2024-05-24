namespace PresentationLayer.DTO.Material
{
    public class NewMaterialDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Saurce { get; set; }
        public int Type { get; set; }
        public int TopicId { get; set; }

    }
}
