namespace PresentationLayer.DTO.Topic
{
    public class NewTopicDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CourceId { get; set; }
        public IFormFile Image { get; set; }


    }
}
